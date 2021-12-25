using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Performance
{
    public class PackedBenchmarkSystem : IEcsRunSystem
    {
        private readonly EcsPackedFilter _filter;

        public PackedBenchmarkSystem(EcsWorld world) =>
            _filter = world.Filter<PositionsComponent>()
                .EndPacked();

        public void Run(EcsSystems systems)
        {
            foreach (var e in _filter)
            {
                ref var testComponent = ref e.Get<PositionsComponent>();
                var direction = Vector3.ClampMagnitude(testComponent.P1 - testComponent.P2, 1f);
                e.GetOrAdd<DirectionComponent>().Direction = direction;
            }
        }
    }
}