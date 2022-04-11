using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class UpdatableComponentView<T> : ComponentView<T> where T : struct
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.OnUpdated<T>();
        }
    }
}