using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EntityExtensions
    {
        public static bool TryGet<T>(this in EcsEntity entity, out T component) where T : struct
        {
            if (entity.IsAlive() && entity.Has<T>())
            {
                component = entity.Get<T>();
                return true;
            }

            component = default;
            return false;
        }
    }
}