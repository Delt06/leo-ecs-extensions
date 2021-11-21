using System;
using System.Collections.Generic;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsEntityRaw = System.Int32;

#else
using Leopotam.Ecs;
using EcsEntityRaw = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    internal static class UpdateEvents
    {
        public delegate EcsFilter FilterFactory(EcsWorld world);
#if LEOECS_EXTENSIONS_LITE
        public delegate IEcsPool PoolFactory(EcsWorld world);
#else
        // ReSharper disable once BuiltInTypeReferenceStyle
        public delegate void EventDelHandler(in EcsEntityRaw entity);
#endif

        public static readonly List<EventMetadata> AllEventsMetadata = new List<EventMetadata>();

        public static void Register<T>() where T : struct
        {
            AllEventsMetadata.Add(new EventMetadata
                {
                    Type = typeof(T),
                    FilterFactory =
#if LEOECS_EXTENSIONS_LITE
                        w => w.Filter<UpdateEvent<T>>().End(),
#else
                        w => w.GetFilter(typeof(EcsFilter<UpdateEvent<T>>)),

#endif
#if LEOECS_EXTENSIONS_LITE
                    PoolFactory = w => w.GetPool<UpdateEvent<T>>(),
#else
                    // ReSharper disable once BuiltInTypeReferenceStyle
                    DelHandler = (in EcsEntityRaw entity) => { entity.Del<UpdateEvent<T>>(); },
#endif
                }
            );
        }

        public struct EventMetadata
        {
            public Type Type;

            public FilterFactory FilterFactory;
#if LEOECS_EXTENSIONS_LITE
            public PoolFactory PoolFactory;
#else
            public EventDelHandler DelHandler;
#endif
        }
    }
}