using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsPackedEntityWithWorldExtensions
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
        public static ref T GetOrAdd<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            return ref pool.GetOrAdd(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform GetTransform(this EcsPackedEntityWithWorld entity) =>
            entity.Get<UnityRef<Transform>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEntityView GetView(this EcsPackedEntityWithWorld entity) => entity.Get<ViewBackRef>().View;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TView GetView<TView>(this EcsPackedEntityWithWorld entity) => entity.Get<ViewBackRef<TView>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Read<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Modify<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(idx);
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ModifyOrAdd<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(idx);
            return ref pool.GetOrAdd(idx);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            return ref pool.Add(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Del<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            var pool = world.GetPool<T>();
            pool.Del(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelModify<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));

            world.GetPool<T>().Del(idx);
            world.GetUpdatesPool<T>().GetOrAdd(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(this EcsPackedEntityWithWorld entity) => entity.Unpack(out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this EcsPackedEntityWithWorld entity)
        {
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(world));
            world.DelEntity(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            var isAlive = entity.Unpack(out var world, out var entityIdx);
#if DEBUG
            if (!isAlive) throw new ArgumentNullException(nameof(entity));
#endif
            return world.GetPool<T>().Has(entityIdx);
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