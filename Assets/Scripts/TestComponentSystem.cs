using Leopotam.Ecs;
using UnityEngine;

public class TestComponentSystem : IEcsRunSystem
{
    private readonly EcsFilter<TestComponent> _filter = default;

    public void Run()
    {
        foreach (var i in _filter)
        {
            var value = _filter.Get1(i).Value;
            Debug.Log($"Test component value: {value}");
        }
    }
}