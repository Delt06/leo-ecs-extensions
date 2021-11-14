using Leopotam.Ecs;
using UnityEngine;

public class TestComponentSystem : IEcsRunSystem
{
    private readonly EcsFilter<TestComponent> _filter = default;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var testComponent = ref _filter.Get1(i);
            ref var value = ref testComponent.Value;
            value += Time.deltaTime;
            Debug.Log($"Test component value: {value}");
        }
    }
}