using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeSyncTransformSystem : IEcsRunSystem
    {
        private readonly EcsFilter _positionFilter;
        private readonly EcsReadOnlyPool<Position> _positions;
        private readonly EcsFilter _rotationFilter;
        private readonly EcsReadOnlyPool<Rotation> _rotations;
        private readonly EcsPool<UnityObjectData<Transform>> _transforms;

        [UsedImplicitly]
        public CubeSyncTransformSystem(EcsWorld world)
        {
            _positionFilter = world.Filter<CubeTag>()
                .Inc<UnityObjectData<Transform>>()
                .IncComponentAndUpdateOf<Position>()
                .End();
            _rotationFilter = world.Filter<CubeTag>()
                .Inc<UnityObjectData<Transform>>()
                .IncComponentAndUpdateOf<Rotation>()
                .End();
            _positions = world.GetPool<Position>().AsReadOnly();
            _rotations = world.GetPool<Rotation>().AsReadOnly();
            _transforms = world.GetPool<UnityObjectData<Transform>>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _positionFilter)
            {
                Transform transform = _transforms.Get(i);
                transform.position = _positions.Read(i).WorldPosition;
            }

            foreach (var i in _rotationFilter)
            {
                Transform transform = _transforms.Get(i);
                transform.rotation = _rotations.Read(i).WorldRotation;
            }
        }
    }
}