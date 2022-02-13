using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public class AutoResetComponentView<T> : ComponentView<T> where T : struct, IEcsAutoReset<T>
    {
        protected override void PreInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PreInitializeEntity(entity);
            StoredComponentValue.AutoReset(ref StoredComponentValue);
        }
    }
}