﻿using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsWorldExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static EcsPackedEntity NewPackedEntity([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.PackEntity(world.NewEntity());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static EcsPackedEntityWithWorld NewPackedEntityWithWorld([NotNull] this EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            var entity = world.NewEntity();
            return world.PackEntityWithWorld(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NewEntityWith<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            var entity = world.NewEntity();
            var pool = world.GetPool<T>();
            return ref pool.Add(entity);
        }
    }
}