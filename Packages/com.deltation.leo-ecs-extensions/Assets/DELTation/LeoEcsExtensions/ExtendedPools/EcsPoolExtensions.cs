using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    public static class EcsPoolExtensions
    {
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
        public static EcsPool<UpdateEvent<T>> GetUpdatesPool<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.GetPool<UpdateEvent<T>>();
        }
    }
}