using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EntityExtensions
    {
#if LEOECS_EXTENSIONS_LITE
        public static EcsPackedFilter EndPacked([NotNull] this EcsWorld.Mask filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return new EcsPackedFilter(filter.End());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask IncComponentAndUpdateOf<T>([NotNull] this EcsWorld.Mask filter) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.Inc<T>().IncUpdateOf<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask IncUpdateOf<T>([NotNull] this EcsWorld.Mask filter) where T : struct
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.Inc<UpdateEvent<T>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask FilterAndIncUpdateOf<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.Filter<T>().Inc<UpdateEvent<T>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask IncTransform([NotNull] this EcsWorld.Mask filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.Inc<UnityObjectData<Transform>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask FilterOnUpdateOf<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.Filter<UpdateEvent<T>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsObservablePool<T> AsObservable<T>([NotNull] this EcsPool<T> pool) where T : struct
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif

            var updatesPool = pool.GetWorld().GetUpdatesPool<T>();
            return new EcsObservablePool<T>(pool, updatesPool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsReadOnlyPool<T> AsReadOnly<T>([NotNull] this EcsPool<T> pool) where T : struct
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif

            return new EcsReadOnlyPool<T>(pool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsReadWritePool<T> AsReadWrite<T>([NotNull] this EcsPool<T> pool) where T : struct
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif

            return new EcsReadWritePool<T>(pool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsReadOnlyPool<T> GetReadOnlyPool<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.GetPool<T>().AsReadOnly();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsObservablePool<T> GetObservablePool<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.GetPool<T>().AsObservable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsReadWritePool<T> GetReadWritePool<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.GetPool<T>().AsReadWrite();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPool<UnityObjectData<Transform>> GetTransformPool([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif

            return world.GetPool<UnityObjectData<Transform>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPool<UpdateEvent<T>> GetUpdatesPool<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.GetPool<UpdateEvent<T>>();
        }

#else
        public static bool TryGet<T>(this in EcsEntity entity, out T component) where T : struct
        {
            if (entity.IsAlive() && entity.Has<T>())
            {
                component = entity.Get<T>();
                return true;
            }

            component = default;
            return false;
        }

#endif
    }
}