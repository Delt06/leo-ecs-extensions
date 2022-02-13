using System.Globalization;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

public class TestComponentTextSystem : IEcsRunSystem
{
    private readonly EcsFilter _filter;
    private readonly EcsReadOnlyPool<TestComponent> _testComponents;

    [UsedImplicitly]
    public TestComponentTextSystem(EcsWorld world)
    {
        _filter = world.FilterAndIncUpdateOf<TestComponent>().End();
        _testComponents = world.GetPool<TestComponent>().AsReadOnly();
    }

    public void Run(EcsSystems systems)
    {
        foreach (var i in _filter)
        {
            ref readonly var testComponent = ref _testComponents.Read(i);
            testComponent.Text.text =
                testComponent.Value.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}