using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityInitializer
    {
        void InitializeEntity(EcsPackedEntityWithWorld entity);
    }
}