using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityView
    {
        EcsWorld World { get; }
        EcsPackedEntityWithWorld GetOrCreateEntity();
        bool TryGetEntity(out EcsPackedEntityWithWorld entity);

        void CreateEntity();
        void DestroyEntity();

        void Destroy();
    }
}