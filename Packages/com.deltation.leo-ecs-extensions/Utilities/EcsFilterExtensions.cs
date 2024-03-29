﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsFilterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsWorld.Mask FilterAndIncUpdateOf<T>([NotNull] this EcsWorld world) where T : struct
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return world.Filter<T>().Inc<UpdateEvent<T>>();
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
        public static EcsWorld.Mask IncTransform([NotNull] this EcsWorld.Mask filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.Inc<UnityRef<Transform>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool IsEmpty([NotNull] this EcsFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.GetEntitiesCount() == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains([NotNull] this EcsFilter filter, EcsPackedEntityWithWorld packedEntity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            if (!packedEntity.Unpack(out var entityWorld, out var entity)) return false;
            if (filter.GetWorld() != entityWorld) return false;

            return filter.Contains(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains([NotNull] this EcsFilter filter, EcsPackedEntityWithWorld packedEntity,
            out int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            if (!packedEntity.Unpack(out var entityWorld, out entity)) return false;
            if (filter.GetWorld() != entityWorld) return false;

            return filter.Contains(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains([NotNull] this EcsFilter filter, EcsPackedEntity packedEntity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            if (!packedEntity.Unpack(filter.GetWorld(), out var entity)) return false;

            return filter.Contains(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains([NotNull] this EcsFilter filter, EcsPackedEntity packedEntity,
            out int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            if (!packedEntity.Unpack(filter.GetWorld(), out entity)) return false;

            return filter.Contains(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            return filter.GetSparseIndex()[entity] > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static int GetSingle([NotNull] this EcsFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            ValidateSingletonFilter(filter);
#endif

            foreach (var i in filter)
            {
                return i;
            }

            throw new InvalidOperationException("Filter contains no entities.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetSingle([NotNull] this EcsFilter filter, out int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            ValidateSingletonFilter(filter);
#endif

            foreach (var i in filter)
            {
                entity = i;
                return true;
            }

            entity = default;
            return false;
        }

        [Conditional("DEBUG")]
        private static void ValidateSingletonFilter(EcsFilter filter)
        {
            if (filter.GetEntitiesCount() >= 2)
                throw new InvalidOperationException(
                    $"Filter contains more than 1 entity ({filter.GetEntitiesCount()})."
                );
        }
    }
}