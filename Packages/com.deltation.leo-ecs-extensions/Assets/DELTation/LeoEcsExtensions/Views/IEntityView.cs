using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityView
    {
        EcsEntity Entity { get; }
        EcsWorld World { get; }
        bool TryGetEntity(out EcsEntity entity);

        void CreateEntity();
        void DestroyEntity();

        void Destroy();
    }
}