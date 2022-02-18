using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using static DELTation.LeoEcsExtensions.Systems.Run.ReflectionPoolFactory;

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
            var foundFilterParameter = false;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType != typeof(EcsFilter)) continue;
                foundFilterParameter = true;
                break;
            }

            if (!foundFilterParameter)
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
                if (Attribute.IsDefined(parameter, typeof(EcsIgnoreAttribute))) continue;
                if (Attribute.IsDefined(parameter, typeof(EcsExcAttribute))) continue;

                firstIncludeIndex = parameterIndex;
                mask = ReflectionFilterFactory.Filter(world, poolComponentType);
                break;
            }

            if (mask == null)
                throw BuiltRunSystemExceptionFactory.NoIncludes();

            for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                if (parameterIndex == firstIncludeIndex) continue;

                var parameter = parameters[parameterIndex];
                if (!IsPool(parameter.ParameterType, out var poolComponentType)) continue;
                if (Attribute.IsDefined(parameter, typeof(EcsIgnoreAttribute))) continue;

                mask = Attribute.IsDefined(parameter, typeof(EcsExcAttribute))
                    ? ReflectionFilterFactory.Exc(mask, poolComponentType)
                    : ReflectionFilterFactory.Inc(mask, poolComponentType);
            }

            filter = mask.End();
            return true;
        }

        private static bool IsPool(Type type, out Type componentType)
        {
            componentType = default;
            if (!type.IsConstructedGenericType) return false;

            var genericType = type.GetGenericTypeDefinition();
            if (genericType != typeof(EcsPool<>) &&
                genericType != typeof(EcsReadOnlyPool<>) &&
                genericType != typeof(EcsReadWritePool<>) &&
                genericType != typeof(EcsObservablePool<>))
                return false;

            var genericArguments = type.GetGenericArguments();
            componentType = genericArguments[0];
            return true;
        }

        private static object ResolveArgument(EcsWorld world, EcsFilter filter, Type parameterType)
        {
            if (parameterType.IsConstructedGenericType)
            {
                var genericType = parameterType.GetGenericTypeDefinition();
                var genericArguments = parameterType.GetGenericArguments();

                if (TryCreatePool(world, genericType, genericArguments, out var pool))
                    return pool;

                if (TryCreateReadOnlyPool(world, parameterType, genericType, genericArguments,
                        out var readOnlyPool
                    ))
                    return readOnlyPool;

                if (TryCreateReadWritePool(world, parameterType, genericType, genericArguments,
                        out var readWritePool
                    ))
                    return readWritePool;

                if (TryCreateObservablePool(world, parameterType, genericType, genericArguments,
                        out var observablePool
                    ))
                    return observablePool;
            }
            else
            {
                if (parameterType == typeof(EcsFilter))
                    return filter;
                if (parameterType == typeof(EcsWorld))
                    return world;
            }

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