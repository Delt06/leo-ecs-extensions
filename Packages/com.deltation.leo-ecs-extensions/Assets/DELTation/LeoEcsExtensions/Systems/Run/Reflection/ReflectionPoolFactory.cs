using System;
using System.Collections.Generic;
using System.Reflection;
using DELTation.LeoEcsExtensions.ExtendedPools;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run.Reflection
{
    internal static class ReflectionPoolFactory
    {
        // https://github.com/Leopotam/ecslite-di/blob/master/src/extensions.cs
        private static readonly MethodInfo WorldGetPoolMethod = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));
        private static readonly Dictionary<Type, MethodInfo>
            GetPoolMethodsCache = new Dictionary<Type, MethodInfo>(256);

        private static readonly object[] ArgumentsArrayOne = new object[1];

        public static bool IsPool(Type type, out Type componentType)
        {
            if (Attribute.IsDefined(type, typeof(EcsPoolAttribute)))
            {
                var ecsPoolAttribute = type.GetCustomAttribute<EcsPoolAttribute>();
                componentType = GetPoolComponentType(type, ecsPoolAttribute);
                return true;
            }

            componentType = default;
            if (!type.IsConstructedGenericType) return false;

            var genericType = type.GetGenericTypeDefinition();
            if (genericType != typeof(EcsPool<>)) return false;

            var genericArguments = type.GetGenericArguments();
            componentType = genericArguments[0];
            return true;
        }

        private static Type GetPoolComponentType(Type poolType, EcsPoolAttribute ecsPoolAttribute)
        {
            var methodName = ecsPoolAttribute.GetComponentTypeMethodName;
            var method = poolType.GetMethod(methodName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            );
            if (method == null)
                throw new ArgumentException($"Type {poolType} does not have a static method named {methodName}.",
                    nameof(poolType)
                );
            return (Type) method.Invoke(null, Array.Empty<object>());
        }

        private static MethodInfo GetGenericGetPoolMethod(Type componentType)
        {
            if (!GetPoolMethodsCache.TryGetValue(componentType, out var pool))
            {
                pool = WorldGetPoolMethod.MakeGenericMethod(componentType);
                GetPoolMethodsCache[componentType] = pool;
            }

            return pool;
        }

        public static bool TryCreatePool(EcsWorld world, Type poolType, out object pool)
        {
            if (Attribute.IsDefined(poolType, typeof(EcsPoolAttribute)))
            {
                var ecsPoolAttribute = poolType.GetCustomAttribute<EcsPoolAttribute>();
                pool = CreatePoolInstance(world, poolType, ecsPoolAttribute);
                return true;
            }

            if (poolType.IsConstructedGenericType && poolType.GetGenericTypeDefinition() == typeof(EcsPool<>))
            {
                pool = GetPool(poolType.GetGenericArguments()[0], world);
                return true;
            }

            pool = default;
            return false;
        }

        private static object CreatePoolInstance(EcsWorld world, Type poolType, EcsPoolAttribute ecsPoolAttribute)
        {
            var methodName = ecsPoolAttribute.CreateInstanceMethodName;
            var method = poolType.GetMethod(methodName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            );
            if (method == null)
                throw new ArgumentException($"Type {poolType} does not have a static method named {methodName}.",
                    nameof(poolType)
                );

            if (method.ReturnType == typeof(void))
                throw new ArgumentException($"Create instance method of {poolType} returns void.", nameof(poolType));

            var parameters = method.GetParameters();
            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(EcsWorld))
                throw new ArgumentException($"Create instance method of {poolType} must have one parameter (EcsWorld).",
                    nameof(poolType)
                );

            ArgumentsArrayOne[0] = world;
            var pool = method.Invoke(null, ArgumentsArrayOne);
            ArgumentsArrayOne[0] = null;
            return pool;
        }

        private static object GetPool(Type componentType, EcsWorld world)
        {
            var getPoolMethod = GetGenericGetPoolMethod(componentType);
            return getPoolMethod.Invoke(world, null);
        }
    }
}