#if LEOECS_EXTENSIONS_LITE
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityInitializer
    {
        void InitializeEntity(EcsPackedEntity entity);
    }
}