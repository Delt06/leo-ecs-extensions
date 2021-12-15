#if LEOECS_EXTENSIONS_LITE

using System;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsFilterExtensions
    {
        public static bool IsEmpty([NotNull] this EcsFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.GetEntitiesCount() == 0;
        }

        public static bool IsEmpty([NotNull] this EcsPackedFilter filter)
        {
#if DEBUG
            if (filter == null) throw new ArgumentNullException(nameof(filter));
#endif
            return filter.GetEntitiesCount() == 0;
        }
    }
}

#endif