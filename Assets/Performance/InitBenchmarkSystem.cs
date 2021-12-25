using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Performance
{
    public class InitBenchmarkSystem : IEcsInitSystem
    {
        private readonly int _entities;
        private readonly EcsWorld _world;

        public InitBenchmarkSystem(EcsWorld world, int entities)
        {
            _world = world;
            _entities = entities;
        }

        public void Init(EcsSystems systems)
        {
            for (var i = 0; i < _entities; i++)
            {
                ref var testComponent = ref _world.NewPackedEntityWithWorld()
                    .Add<PositionsComponent>();
                const float radius = 100f;
                testComponent.P1 = Random.insideUnitSphere * radius;
                testComponent.P2 = Random.insideUnitSphere * radius;
            }
        }
    }
}