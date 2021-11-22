#if LEOECS_EXTENSIONS_LITE
using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Compatibility;
using JetBrains.Annotations;
using Leopotam.EcsLite;

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
    }
}

#endif