using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeTranslationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<Position> _positions;
        private EcsPool<PositionWriteRequired> _writes;


        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<Position>().Inc<CubeTag>().End();
            _positions = world.GetPool<Position>();
            _writes = world.GetPool<PositionWriteRequired>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var position = ref _positions.Get(i);
                var newWorldPosition = position.WorldPosition;
                newWorldPosition.y = Mathf.Sin(Time.time);
                _writes.UpdatePosition(i, ref position, newWorldPosition);
            }
        }
    }
}