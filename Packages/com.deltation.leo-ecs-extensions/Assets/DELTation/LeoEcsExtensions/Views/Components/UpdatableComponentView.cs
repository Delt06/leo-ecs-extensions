using DELTation.LeoEcsExtensions.Components;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using Leopotam.Ecs;
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class UpdatableComponentView<T> : ComponentView<T> where T : struct
    {
        protected override void PostInitializeEntity(EcsPackedEntity entity)
        {
            base.PostInitializeEntity(entity);
            entity.OnUpdated<T>();
        }
    }
}