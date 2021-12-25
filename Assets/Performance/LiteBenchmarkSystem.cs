using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Performance
{
    public class LiteBenchmarkSystem : IEcsRunSystem
    {
        private readonly EcsPool<DirectionComponent> _directions;
        private readonly EcsFilter _filter;
        private readonly EcsPool<PositionsComponent> _positions;

        public LiteBenchmarkSystem(EcsWorld world)
        {
            _positions = world.GetPool<PositionsComponent>();
            _directions = world.GetPool<DirectionComponent>();
            _filter = world.Filter<PositionsComponent>().End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var position = ref _positions.Get(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                _directions.GetOrAdd(i).Direction = direction;
            }
        }
    }
}