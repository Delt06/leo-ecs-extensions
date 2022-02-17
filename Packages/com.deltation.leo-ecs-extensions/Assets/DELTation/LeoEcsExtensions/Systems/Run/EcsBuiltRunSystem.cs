using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using static DELTation.LeoEcsExtensions.Systems.Run.ReflectionPoolFactory;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public readonly struct EcsBuiltRunSystem
    {
#pragma warning disable CS0414
        private readonly bool _isValid;
#pragma warning restore CS0414
        private readonly EcsFilter _filter;
        private readonly EcsWorld _world;
        private readonly Delegate _delegate;
        private readonly object[] _arguments;


        public EcsBuiltRunSystem([NotNull] EcsFilter filter, [NotNull] Delegate @delegate)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _world = filter.GetWorld();
            _delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));

            var method = _delegate.Method;
            if (method.ReturnType != typeof(void))
                throw BuiltRunSystemExceptionFactory.NonVoidReturnType(nameof(@delegate));

            var parameters = method.GetParameters();
            _arguments = new object[parameters.Length];
            _isValid = false;

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter.IsIn)
                    throw BuiltRunSystemExceptionFactory.InParameter(i, nameof(@delegate));
                if (parameter.IsOut)
                    throw BuiltRunSystemExceptionFactory.OutParameter(i, nameof(@delegate));

                var parameterType = parameter.ParameterType;
                if (parameterType.IsByRef)
                    throw BuiltRunSystemExceptionFactory.RefParameter(i, nameof(@delegate));

                _arguments[i] = ResolveArgument(parameterType);
            }

            _isValid = true;
        }

        private object ResolveArgument(Type parameterType)
        {
            if (parameterType.IsConstructedGenericType)
            {
                var genericType = parameterType.GetGenericTypeDefinition();
                var genericArguments = parameterType.GetGenericArguments();

                if (TryCreatePool(_world, genericType, genericArguments, out var pool))
                    return pool;

                if (TryCreateReadOnlyPool(_world, parameterType, genericType, genericArguments,
                        out var readOnlyPool
                    ))
                    return readOnlyPool;

                if (TryCreateReadWritePool(_world, parameterType, genericType, genericArguments,
                        out var readWritePool
                    ))
                    return readWritePool;

                if (TryCreateObservablePool(_world, parameterType, genericType, genericArguments,
                        out var observablePool
                    ))
                    return observablePool;
            }
            else
            {
                if (parameterType == typeof(EcsFilter))
                    return _filter;
                if (parameterType == typeof(EcsWorld))
                    return _world;
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