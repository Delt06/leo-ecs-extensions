using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

public class TestComponentView : ComponentView<TestComponent>
{
    protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
    {
        base.PostInitializeEntity(entity);
        entity.OnUpdated<TestComponent>();
    }
}