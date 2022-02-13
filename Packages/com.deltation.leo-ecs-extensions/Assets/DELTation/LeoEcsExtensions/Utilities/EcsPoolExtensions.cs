using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsPoolExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPool<UnityObjectData<Transform>> GetTransformPool([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif

            return world.GetPool<UnityObjectData<Transform>>();
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

        public static ref T AddNewEntity<T>(this EcsPool<T> pool) where T : struct
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif
            return ref pool.Add(pool.GetWorld().NewEntity());
        }
    }
}