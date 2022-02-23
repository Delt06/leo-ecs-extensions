using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using DELTation.LeoEcsExtensions.Systems.Run.Reflection;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using static DELTation.LeoEcsExtensions.Systems.Run.Reflection.ReflectionPoolFactory;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public readonly struct EcsCallback
    {
#pragma warning disable CS0414
        private readonly bool _isValid;
#pragma warning restore CS0414

        private readonly Delegate _delegate;
        private readonly object[] _arguments;


        public EcsCallback([NotNull] EcsWorld world, [NotNull] MethodInfo method, [NotNull] object methodTarget)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (methodTarget == null) throw new ArgumentNullException(nameof(methodTarget));

            if (method.ReturnType != typeof(void))
                throw BuiltRunSystemExceptionFactory.NonVoidReturnType(nameof(method));

            var parameters = method.GetParameters();
            if (!TryCreateFilter(world, parameters, out var filter))
                filter = null;
            _arguments = new object[parameters.Length];
            _isValid = false;

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter.IsIn)
                    throw BuiltRunSystemExceptionFactory.InParameter(i, nameof(method));
                if (parameter.IsOut)
                    throw BuiltRunSystemExceptionFactory.OutParameter(i, nameof(method));

                var parameterType = parameter.ParameterType;
                if (parameterType.IsByRef)
                    throw BuiltRunSystemExceptionFactory.RefParameter(i, nameof(method));

                _arguments[i] = ResolveArgument(world, filter, parameterType);
            }

            _delegate = ReflectionDelegateFactory.CreateActionDelegate(method, methodTarget);
            _isValid = true;
        }

        private static bool TryCreateFilter(EcsWorld world, ParameterInfo[] parameters, out EcsFilter filter)
        {
            var filterParameterIndex = FindFilterParameter(parameters);

            if (filterParameterIndex == -1)
            {
                filter = default;
                return false;
            }

            EcsWorld.Mask mask = null;
            var firstIncludeIndex = -1;

            for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                var parameter = parameters[parameterIndex];
                if (!IsPool(parameter.ParameterType, out var poolComponentType)) continue;
                if (IsIncIgnored(parameter)) continue;

                firstIncludeIndex = parameterIndex;
                mask = ReflectionFilterFactory.Filter(world, poolComponentType);

                if (Attribute.IsDefined(parameter, typeof(EcsIncUpdate)))
                    mask = ReflectionFilterFactory.Inc(mask,
                        ReflectionFilterFactory.GetUpdateEventType(poolComponentType)
                    );

                break;
            }

            var filterParameter = parameters[filterParameterIndex];
            ProcessExtraIncludes(filterParameter, world, ref mask);

            if (mask == null)
                throw BuiltRunSystemExceptionFactory.NoIncludes();

            ProcessExcludes(filterParameter, mask);
            ProcessExcludedPools(parameters, mask);

            for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                if (parameterIndex == firstIncludeIndex) continue;

                var parameter = parameters[parameterIndex];
                if (!IsPool(parameter.ParameterType, out var poolComponentType)) continue;
                if (IsIncIgnored(parameter)) continue;

                mask = ReflectionFilterFactory.Inc(mask, poolComponentType);

                if (Attribute.IsDefined(parameter, typeof(EcsIncUpdate)))
                    mask = ReflectionFilterFactory.Inc(mask,
                        ReflectionFilterFactory.GetUpdateEventType(poolComponentType)
                    );
            }

            filter = mask.End();
            return true;
        }


        private static bool IsIncIgnored(ParameterInfo parameter) =>
            Attribute.IsDefined(parameter, typeof(EcsIgnoreIncAttribute)) ||
            Attribute.IsDefined(parameter, typeof(EcsExcPoolAttribute)) ||
            IsPoolIgnoredByDefault(parameter.ParameterType);

        private static int FindFilterParameter(ParameterInfo[] parameters)
        {
            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                if (parameter.ParameterType != typeof(EcsFilter)) continue;
                return index;
            }

            return -1;
        }

        private static void ProcessExcludes(ParameterInfo filterParameter, EcsWorld.Mask mask)
        {
            if (!Attribute.IsDefined(filterParameter, typeof(EcsExcAttribute))) return;

            var ecsExcAttributes = filterParameter.GetCustomAttributes<EcsExcAttribute>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var ecsExcAttribute in ecsExcAttributes)
            {
                var type = ecsExcAttribute.Type;
                mask = ReflectionFilterFactory.Exc(mask, type);
            }
        }

        private static void ProcessExcludedPools(ParameterInfo[] parameters, EcsWorld.Mask mask)
        {
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                if (!IsPool(parameterType, out var componentType)) continue;

                var ecsExcPoolAttribute = parameter.GetCustomAttribute<EcsExcPoolAttribute>();
                if (ecsExcPoolAttribute == null) continue;

                ReflectionFilterFactory.Exc(mask, componentType);
            }
        }

        private static void ProcessExtraIncludes(ParameterInfo filterParameter, EcsWorld world, ref EcsWorld.Mask mask)
        {
            if (!Attribute.IsDefined(filterParameter, typeof(EcsIncAttribute))) return;

            var ecsIncAttributes = filterParameter.GetCustomAttributes<EcsIncAttribute>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var ecsIncAttribute in ecsIncAttributes)
            {
                var type = ecsIncAttribute.Type;
                mask = mask == null
                    ? ReflectionFilterFactory.Filter(world, type)
                    : ReflectionFilterFactory.Inc(mask, type);
            }
        }

        private static object ResolveArgument(EcsWorld world, EcsFilter filter, Type parameterType)
        {
            if (TryCreatePool(world, parameterType, out var pool))
                return pool;

            if (parameterType == typeof(EcsFilter))
                return filter;
            if (parameterType == typeof(EcsWorld))
                return world;

            throw BuiltRunSystemExceptionFactory.InvalidParameterType(parameterType);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run()
        {
#if DEBUG
            if (!_isValid)
                throw new InvalidOperationException("BuiltRunSystem was not properly created.");
#endif
            _delegate.DynamicInvoke(_arguments);
        }
    }
}