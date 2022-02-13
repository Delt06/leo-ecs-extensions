using System;
using System.Runtime.CompilerServices;
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
            if (!entity.IsAlive()) throw new ArgumentNullException(nameof(entity));
#endif
            entity.GetOrAdd<UpdateEvent<T>>();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetComponentTypesCompatible(this EcsPackedEntityWithWorld entity)
        {
            var isAlive = entity.Unpack(out var world, out var entityIdx);
#if DEBUG
            if (!isAlive) throw new ArgumentNullException(nameof(entity));
#endif
            Type[] componentTypes = null;
            world.GetComponentTypes(entityIdx, ref componentTypes);
            return componentTypes;
        }
    }
}