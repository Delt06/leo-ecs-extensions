using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class LiteFilterAccessBenchmarkSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;

        public LiteFilterAccessBenchmarkSystem(EcsWorld world) =>
            _filter = world.Filter<PositionsComponent>().End();

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get<PositionsComponent>(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                _filter.GetOrAdd<DirectionComponent>(i).Direction = direction;
            }
        }
    }
}