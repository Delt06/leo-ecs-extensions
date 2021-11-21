using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Systems
{
    [SystemComponentAccess(typeof(UpdateEvent<>), ComponentAccessType.ReadWrite)]
    internal class OneFrameUpdateEventsSystem : IEcsRunSystem
    {
        private readonly IDictionary<Type, EcsFilter> _filtersCache = new Dictionary<Type, EcsFilter>();
#if LEOECS_EXTENSIONS_LITE
        private readonly IDictionary<Type, IEcsPool> _poolsCache = new Dictionary<Type, IEcsPool>();
#else
        private readonly EcsWorld _world = default;
#endif

#if LEOECS_EXTENSIONS_LITE
        public void Run(EcsSystems systems)
#else
        public void Run()
#endif

        {
#if LEOECS_EXTENSIONS_LITE
            var world = systems.GetWorld();
#else
            var world = _world;
#endif

            // ReSharper disable once ForCanBeConvertedToForeach
            var eventsCount = UpdateEvents.AllEventsMetadata.Count;
            for (var index = 0; index < eventsCount; index++)
            {
                var metadata = UpdateEvents.AllEventsMetadata[index];
                var filter = GetFilter(metadata, world);
#if LEOECS_EXTENSIONS_LITE
                var pool = GetPool(metadata, world);
#endif


#if LEOECS_EXTENSIONS_LITE
                foreach (var i in filter)
                {
                    pool.Del(i);
                }
#else
                for (var idx = filter.GetEntitiesCount() - 1; idx >= 0; idx--)
                {
                    metadata.DelHandler(filter.GetEntity(idx));
                }
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EcsFilter GetFilter(in UpdateEvents.EventMetadata metadata, EcsWorld world)
        {
            if (_filtersCache.TryGetValue(metadata.Type, out var filter))
                return filter;
            return _filtersCache[metadata.Type] = metadata.FilterFactory(world);
        }

#if LEOECS_EXTENSIONS_LITE

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEcsPool GetPool(in UpdateEvents.EventMetadata metadata, EcsWorld world)
        {
            if (_poolsCache.TryGetValue(metadata.Type, out var pool))
                return pool;
            return _poolsCache[metadata.Type] = metadata.PoolFactory(world);
        }

#endif
    }
}