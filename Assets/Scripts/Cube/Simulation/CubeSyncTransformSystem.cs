using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Pools;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeSyncTransformPrototypeSystem : IEcsRunSystem
    {
        private readonly EcsFilter _positionFilter;
        private readonly EcsFilter _rotationFilter;

        public CubeSyncTransformPrototypeSystem(EcsWorld world)
        {
            _positionFilter = world.Filter<CubeTag>()
                .Inc<UnityObjectData<Transform>>()
                .IncComponentAndUpdateOf<Position>()
                .End();

            _rotationFilter = world.Filter<CubeTag>()
                .Inc<UnityObjectData<Transform>>()
                .IncComponentAndUpdateOf<Rotation>()
                .End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var e in _positionFilter)
            {
                var transform = _positionFilter.GetTransform(e);
                transform.position = _positionFilter.Read<Position>(e).WorldPosition;
            }

            foreach (var e in _rotationFilter)
            {
                var transform = _positionFilter.GetTransform(e);
                transform.rotation = _positionFilter.Read<Rotation>(e).WorldRotation;
            }
        }
    }

    public class CubeSyncTransformSystem : IEcsRunSystem
    {
        private readonly EcsFilter _positionFilter;
        private readonly EcsReadOnlyPool<Position> _positions;
        private readonly EcsFilter _rotationFilter;
        private readonly EcsReadOnlyPool<Rotation> _rotations;
        private readonly EcsReadOnlyPool<UnityObjectData<Transform>> _transforms;

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
            _transforms = world.GetPool<UnityObjectData<Transform>>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _positionFilter)
            {
                Transform transform = _transforms.Read(i);
                transform.position = _positions.Read(i).WorldPosition;
            }

            foreach (var i in _rotationFilter)
            {
                Transform transform = _transforms.Read(i);
                transform.rotation = _rotations.Read(i).WorldRotation;
            }
        }
    }
}