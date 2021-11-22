using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Compatibility;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    public static class EcsEntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnUpdated<T>(this EcsPackedEntity entity) where T : struct
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
#endif
            entity.GetCompatible<UpdateEvent<T>>();
        }

#if LEOECS_EXTENSIONS_LITE
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
#endif
    }
}