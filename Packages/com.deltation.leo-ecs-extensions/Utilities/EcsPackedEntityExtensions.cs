using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsPackedEntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool IsAlive(this EcsPackedEntity entity, EcsWorld world)
        {
#if DEBUG
            if (world == null) throw new ArgumentNullException(nameof(world));
#endif
            return entity.Unpack(world, out _);
        }
    }
}