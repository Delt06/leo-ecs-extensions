#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using Leopotam.Ecs;
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public class AutoResetComponentView<T> : ComponentView<T> where T : struct, IEcsAutoReset<T>
    {
        protected override void PreInitializeEntity(EcsPackedEntity entity)
        {
            base.PreInitializeEntity(entity);
            StoredComponentValue.AutoReset(ref StoredComponentValue);
        }
    }
}