﻿using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

public class TestComponentSystem : IEcsRunSystem
{
    private readonly EcsFilter _filter;
    private readonly EcsPool<TestComponent> _pool;

    [UsedImplicitly]
    public TestComponentSystem(EcsWorld world)
    {
        _filter = world.Filter<TestComponent>().End();
        _pool = world.GetPool<TestComponent>();
    }

    public void Run(EcsSystems systems)
    {
        foreach (var i in _filter)
        {
            ref var testComponent = ref _pool.Get(i);
            ref var value = ref testComponent.Value;
            value += Time.deltaTime;
            Debug.Log($"Test component value: {value}");
        }
    }
}