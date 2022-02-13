using DELTation.LeoEcsExtensions.ExtendedPools;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class ExtendedBenchmarkSystem : IEcsRunSystem
    {
        private readonly EcsReadWritePool<DirectionComponent> _directions;
        private readonly EcsFilter _filter;
        private readonly EcsReadWritePool<PositionsComponent> _positions;

        public ExtendedBenchmarkSystem(EcsWorld world)
        {
            _positions = world.GetReadWritePool<PositionsComponent>();
            _directions = world.GetReadWritePool<DirectionComponent>();
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