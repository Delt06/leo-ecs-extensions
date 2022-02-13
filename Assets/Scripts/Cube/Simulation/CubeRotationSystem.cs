using Cube.Components;
using DELTation.LeoEcsExtensions.Pools;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsObservablePool<Rotation> _rotations;

        public CubeRotationSystem(EcsWorld world)
        {
            _filter = world.Filter<Rotation>().Inc<CubeTag>().End();
            _rotations = world.GetPool<Rotation>().AsObservable();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var rotation = ref _rotations.Modify(i);
                const float rotationSpeed = 90f;
                rotation.WorldRotation = Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up) *
                                         rotation.WorldRotation;
            }
        }
    }
}