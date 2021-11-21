#if LEOECS_EXTENSIONS_LITE
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;
using EcsWorld = Leopotam.EcsLite.EcsWorld;

#else
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
using EcsWorld = Leopotam.Ecs.EcsWorld;
#endif

namespace DELTation.LeoEcsExtensions.Views
{
    public interface IEntityView
    {
        EcsWorld World { get; }
        EcsPackedEntity GetOrCreateEntity();
        bool TryGetEntity(out EcsPackedEntity entity);

        void CreateEntity();
        void DestroyEntity();

        void Destroy();
    }
}