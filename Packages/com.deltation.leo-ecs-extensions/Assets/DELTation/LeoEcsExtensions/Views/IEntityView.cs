using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityView
    {
        EcsWorld World { get; }
        EcsEntity GetOrCreateEntity();
        bool TryGetEntity(out EcsEntity entity);

        void CreateEntity();
        void DestroyEntity();

        void Destroy();
    }
}