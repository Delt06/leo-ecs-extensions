using System;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Components
{
    public static class EcsEntityExtensions
    {
        public static void OnUpdated<T>(this EcsEntity entity) where T : struct
        {
#if DEBUG
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
#endif
            entity.Get<UpdateEvent<T>>();
        }
    }
}