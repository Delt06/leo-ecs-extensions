using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public class AutoResetComponentView<T> : ComponentView<T> where T : struct, IEcsAutoReset<T>
    {
        protected override void PreInitializeEntity(EcsEntity entity)
        {
            base.PreInitializeEntity(entity);
            StoreComponentValue.AutoReset(ref StoreComponentValue);
        }
    }
}