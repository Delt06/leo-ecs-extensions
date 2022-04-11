using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems
{
    [SystemComponentAccess(typeof(UpdateEvent<>), ComponentAccessType.Unstructured)]
    internal class OneFrameUpdateEventsSystem : IEcsRunSystem
    {
        private readonly IDictionary<Type, EcsFilter> _filtersCache = new Dictionary<Type, EcsFilter>();
        private readonly IDictionary<Type, IEcsPool> _poolsCache = new Dictionary<Type, IEcsPool>();

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            // ReSharper disable once ForCanBeConvertedToForeach
            var eventsCount = UpdateEvents.AllEventsMetadata.Count;
            for (var index = 0; index < eventsCount; index++)
            {
                var metadata = UpdateEvents.AllEventsMetadata[index];
                var filter = GetFilter(metadata, world);
                var pool = GetPool(metadata, world);


                foreach (var i in filter)
                {
                    pool.Del(i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EcsFilter GetFilter(in UpdateEvents.EventMetadata metadata, EcsWorld world)
        {
            if (_filtersCache.TryGetValue(metadata.Type, out var filter))
                return filter;
            return _filtersCache[metadata.Type] = metadata.FilterFactory(world);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEcsPool GetPool(in UpdateEvents.EventMetadata metadata, EcsWorld world)
        {
            if (_poolsCache.TryGetValue(metadata.Type, out var pool))
                return pool;
            return _poolsCache[metadata.Type] = metadata.PoolFactory(world);
        }
    }
}