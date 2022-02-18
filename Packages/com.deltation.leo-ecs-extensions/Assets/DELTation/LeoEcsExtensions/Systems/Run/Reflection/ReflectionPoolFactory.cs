using System;
using System.Collections.Generic;
using System.Reflection;
using DELTation.LeoEcsExtensions.Components;
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
        private static readonly object[] ArgumentsArrayTwo = new object[2];

        private static MethodInfo GetGenericGetPoolMethod(Type componentType)
        {
            if (!GetPoolMethodsCache.TryGetValue(componentType, out var pool))
            {
                pool = WorldGetPoolMethod.MakeGenericMethod(componentType);
                GetPoolMethodsCache[componentType] = pool;
            }

            return pool;
        }

        public static bool TryCreatePool(EcsWorld world, Type genericType, Type[] genericArguments, out object pool)
        {
            if (genericType == typeof(EcsPool<>))
            {
                pool = GetPool(genericArguments[0], world);
                return true;
            }

            pool = default;
            return false;
        }

        public static bool TryCreateReadOnlyPool(EcsWorld world, Type parameterType, Type genericType,
            Type[] genericArguments,
            out object pool)
        {
            if (genericType == typeof(EcsReadOnlyPool<>))
            {
                var innerPool = GetPool(genericArguments[0], world);
                pool = CreateInstanceViaNonPublicConstructor(parameterType, innerPool);
                return true;
            }

            pool = default;
            return false;
        }

        public static bool TryCreateReadWritePool(EcsWorld world, Type parameterType, Type genericType,
            Type[] genericArguments,
            out object pool)
        {
            if (genericType == typeof(EcsReadWritePool<>))
            {
                var innerPool = GetPool(genericArguments[0], world);
                pool = CreateInstanceViaNonPublicConstructor(parameterType, innerPool);
                return true;
            }

            pool = default;
            return false;
        }

        public static bool TryCreateObservablePool(EcsWorld world, Type parameterType, Type genericType,
            Type[] genericArguments,
            out object pool)
        {
            if (genericType == typeof(EcsObservablePool<>))
            {
                var componentType = genericArguments[0];
                var innerPool = GetPool(componentType, world);

                var updateEventGenericType = typeof(UpdateEvent<>);
                var updateEventType = updateEventGenericType.MakeGenericType(componentType);
                var updateEventPool = GetPool(updateEventType, world);

                pool = CreateInstanceViaNonPublicConstructor(parameterType, innerPool, updateEventPool);
                return true;
            }

            pool = default;
            return false;
        }

        private static object CreateInstanceViaNonPublicConstructor(Type type, object argument)
        {
            ArgumentsArrayOne[0] = argument;
            return CreateInstanceViaNonPublicConstructor(type, ArgumentsArrayOne);
        }

        private static object CreateInstanceViaNonPublicConstructor(Type type, object argument1, object argument2)
        {
            ArgumentsArrayTwo[0] = argument1;
            ArgumentsArrayTwo[1] = argument2;
            return CreateInstanceViaNonPublicConstructor(type, ArgumentsArrayTwo);
        }

        private static object CreateInstanceViaNonPublicConstructor(Type type, object[] arguments) =>
            Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, arguments, null);

        private static object GetPool(Type componentType, EcsWorld world)
        {
            var getPoolMethod = GetGenericGetPoolMethod(componentType);
            return getPoolMethod.Invoke(world, null);
        }
    }
}