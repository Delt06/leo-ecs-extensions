using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Compatibility;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Components
{
    public static class EcsEntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnUpdated<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
#endif
            entity.GetCompatible<UpdateEvent<T>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrAdd<T>([NotNull] this EcsPool<T> pool, int entity) where T : struct
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif
            if (pool.Has(entity))
                return ref pool.Get(entity);
            return ref pool.Add(entity);
        }
    }
}