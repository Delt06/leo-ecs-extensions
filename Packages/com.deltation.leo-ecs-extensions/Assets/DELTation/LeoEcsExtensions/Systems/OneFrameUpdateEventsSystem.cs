using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Systems
{
    internal class OneFrameUpdateEventsSystem : IEcsRunSystem
    {
        private readonly IDictionary<Type, EcsFilter> _filtersCache = new Dictionary<Type, EcsFilter>();
        private readonly EcsWorld _world = default;

        public void Run()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            var eventsCount = UpdateEvents.AllEventsMetadata.Count;
            for (var index = 0; index < eventsCount; index++)
            {
                var metadata = UpdateEvents.AllEventsMetadata[index];
                var filter = GetFilter(metadata);

                for (var idx = filter.GetEntitiesCount() - 1; idx >= 0; idx--)
                {
                    metadata.DelHandler(filter.GetEntity(idx));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EcsFilter GetFilter(in UpdateEvents.EventMetadata metadata)
        {
            if (_filtersCache.TryGetValue(metadata.Type, out var filter))
                return filter;
            return _filtersCache[metadata.Type] = _world.GetFilter(metadata.OneFrameFilterType);
        }
    }
}