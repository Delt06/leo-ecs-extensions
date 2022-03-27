using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Components
{
    internal static class UpdateEvents
    {
        public delegate EcsFilter FilterFactory(EcsWorld world);

        public delegate IEcsPool PoolFactory(EcsWorld world);

        public static readonly List<EventMetadata> AllEventsMetadata = new List<EventMetadata>();

        public static void Register<T>() where T : struct
        {
            AllEventsMetadata.Add(new EventMetadata
                {
                    Type = typeof(T),
                    FilterFactory =
                        w => w.Filter<UpdateEvent<T>>().End(),
                    PoolFactory = w => w.GetPool<UpdateEvent<T>>(),
                }
            );
        }

        public struct EventMetadata
        {
            public Type Type;

            public FilterFactory FilterFactory;
            public PoolFactory PoolFactory;
        }
    }
}