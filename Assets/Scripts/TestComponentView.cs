using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.Ecs;

public class TestComponentView : ComponentView<TestComponent>
{
    protected override void PostInitializeEntity(EcsEntity entity)
    {
        base.PostInitializeEntity(entity);
        entity.OnUpdated<TestComponent>();
    }
}