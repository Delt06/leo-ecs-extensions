using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsFilterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty([NotNull] this EcsFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.GetEntitiesCount() == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static bool Contains([NotNull] this EcsFilter filter, int entity)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif

            return filter.GetSparseIndex()[entity] > 0;
        }
    }
}