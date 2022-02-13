using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Pools;
using DELTation.LeoEcsExtensions.Views;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsFilterComponentAccess
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrAdd<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            return ref pool.GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            return ref pool.Get(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Read<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            return ref pool.Get(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Modify<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var world = filter.GetWorld();
            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(entity);
            return ref pool.Get(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ModifyOrAdd<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var world = filter.GetWorld();
            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(entity);
            return ref pool.GetOrAdd(entity);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            return ref pool.Add(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Del<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            pool.Del(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelModify<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var world = filter.GetWorld();
            world.GetPool<T>().Del(entity);
            world.GetUpdatesPool<T>().GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            filter.GetWorld().DelEntity(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>([NotNull] this EcsFilter filter, int entity) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            var pool = filter.GetWorld().GetPool<T>();
            return pool.Has(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform GetTransform([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            return filter.Get<UnityObjectData<Transform>>(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEntityView GetView([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            return filter.Get<ViewBackRef>(entity).View;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TView GetView<TView>([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            return filter.Get<ViewBackRef<TView>>(entity);
        }
    }
}