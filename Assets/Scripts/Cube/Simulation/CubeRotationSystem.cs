using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeRotationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<Rotation> _rotations;
        private EcsPool<RotationWriteRequired> _writes;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<Rotation>().Inc<CubeTag>().End();
            _rotations = world.GetPool<Rotation>();
            _writes = world.GetPool<RotationWriteRequired>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var rotation = ref _rotations.Get(i);
                const float rotationSpeed = 90f;
                var newWorldRotation = Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up) *
                                       rotation.WorldRotation;

                _writes.UpdateRotation(i, ref rotation, newWorldRotation);
            }
        }
    }
}