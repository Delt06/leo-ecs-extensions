using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    internal static class ReflectionFilterFactory
    {
        // https://github.com/Leopotam/ecslite-di/blob/master/src/extensions.cs
        private static readonly MethodInfo WorldFilterMethod = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));
        private static readonly MethodInfo MaskIncMethod = typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));
        private static readonly MethodInfo MaskExcMethod = typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));

        private static readonly Dictionary<Type, MethodInfo> FilterMethodsCache = new Dictionary<Type, MethodInfo>(256);
        private static readonly Dictionary<Type, MethodInfo>
            MaskIncMethodsCache = new Dictionary<Type, MethodInfo>(256);
        private static readonly Dictionary<Type, MethodInfo>
            MaskExcMethodsCache = new Dictionary<Type, MethodInfo>(256);

        public static EcsWorld.Mask Filter([NotNull] EcsWorld world, [NotNull] Type componentType)
        {
            if (world == null) throw new ArgumentNullException(nameof(world));
            if (componentType == null) throw new ArgumentNullException(nameof(componentType));

            return (EcsWorld.Mask) GetGenericFilterMethod(componentType).Invoke(world, null);
        }

        public static EcsWorld.Mask Inc([NotNull] EcsWorld.Mask mask, [NotNull] Type componentType)
        {
            if (mask == null) throw new ArgumentNullException(nameof(mask));
            if (componentType == null) throw new ArgumentNullException(nameof(componentType));

            return (EcsWorld.Mask) GetGenericMaskIncMethod(componentType).Invoke(mask, null);
        }

        public static EcsWorld.Mask Exc([NotNull] EcsWorld.Mask mask, [NotNull] Type componentType)
        {
            if (mask == null) throw new ArgumentNullException(nameof(mask));
            if (componentType == null) throw new ArgumentNullException(nameof(componentType));

            return (EcsWorld.Mask) GetGenericMaskExcMethod(componentType).Invoke(mask, null);
        }


        private static MethodInfo GetGenericFilterMethod(Type componentType)
        {
            if (!FilterMethodsCache.TryGetValue(componentType, out var filter))
            {
                filter = WorldFilterMethod.MakeGenericMethod(componentType);
                FilterMethodsCache[componentType] = filter;
            }

            return filter;
        }

        private static MethodInfo GetGenericMaskIncMethod(Type componentType)
        {
            if (!MaskIncMethodsCache.TryGetValue(componentType, out var inc))
            {
                inc = MaskIncMethod.MakeGenericMethod(componentType);
                MaskIncMethodsCache[componentType] = inc;
            }

            return inc;
        }

        private static MethodInfo GetGenericMaskExcMethod(Type componentType)
        {
            if (!MaskExcMethodsCache.TryGetValue(componentType, out var exc))
            {
                exc = MaskExcMethod.MakeGenericMethod(componentType);
                MaskExcMethodsCache[componentType] = exc;
            }

            return exc;
        }
    }
}