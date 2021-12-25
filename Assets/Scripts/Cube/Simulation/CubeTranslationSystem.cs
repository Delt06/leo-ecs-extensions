using Cube.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeTranslationSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsObservablePool<Position> _positions;

        public CubeTranslationSystem(EcsWorld world)
        {
            _filter = world.Filter<Position>().Inc<CubeTag>().End();
            _positions = world.GetPool<Position>().AsObservable();
        }

        public void Run(EcsSystems systems)
        {
            if (Time.time % 2f >= 1f) return;

            foreach (var i in _filter)
            {
                ref var position = ref _positions.Modify(i);
                position.WorldPosition.y = Mathf.Sin(Time.time);
            }
        }
    }
}