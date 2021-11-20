using System;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Components
{
    internal static class UpdateEvents
    {
        public delegate void EventDelHandler(in EcsEntity entity);

        public static readonly List<EventMetadata> AllEventsMetadata = new List<EventMetadata>();

        public static void Register<T>() where T : struct
        {
            AllEventsMetadata.Add(new EventMetadata
                {
                    Type = typeof(T),
                    OneFrameFilterType = typeof(EcsFilter<UpdateEvent<T>>),
                    DelHandler = (in EcsEntity entity) => entity.Del<UpdateEvent<T>>(),
                }
            );
        }

        public struct EventMetadata
        {
            public Type Type;
            public Type OneFrameFilterType;
            public EventDelHandler DelHandler;
        }
    }
}