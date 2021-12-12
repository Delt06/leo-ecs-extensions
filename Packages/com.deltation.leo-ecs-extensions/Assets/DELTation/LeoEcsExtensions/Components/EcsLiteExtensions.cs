#if LEOECS_EXTENSIONS_LITE
using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Utilities;
using DELTation.LeoEcsExtensions.Views;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
    public static class EcsLiteExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntityWithWorld NewPackedEntityWithWorld([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.NewPackedEntityCompatible();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntity NewPackedEntity([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.PackEntity(world.NewEntity());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrAdd<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            return ref pool.GetOrAdd(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform GetTransform(this EcsPackedEntityWithWorld entity) =>
            entity.Get<UnityObjectData<Transform>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEntityView GetView(this EcsPackedEntityWithWorld entity) => entity.Get<ViewBackRef>().View;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TView GetView<TView>(this EcsPackedEntityWithWorld entity) => entity.Get<ViewBackRef<TView>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Read<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Modify<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(idx);
            return ref pool.Get(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ModifyOrAdd<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            world.GetUpdatesPool<T>().GetOrAdd(idx);
            return ref pool.GetOrAdd(idx);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
#if DEBUG
            if (!entity.Unpack(out var world, out var idx)) throw new ArgumentNullException(nameof(entity));
#endif

            var pool = world.GetPool<T>();
            return ref pool.Add(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(this EcsPackedEntity entity, EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return entity.Unpack(world, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(this EcsPackedEntityWithWorld entity) => entity.IsAliveCompatible();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this EcsPackedEntityWithWorld entity) where T : struct => entity.HasCompatible<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this EcsFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.GetEntitiesCount() == 0;
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

#endif