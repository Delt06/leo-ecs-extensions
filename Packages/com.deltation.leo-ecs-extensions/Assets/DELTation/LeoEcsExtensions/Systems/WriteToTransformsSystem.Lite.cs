#if LEOECS_EXTENSIONS_LITE
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Systems
{
    public sealed class WriteToTransformsSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _dirtyPositionsFilter;
        private EcsFilter _dirtyRotationsFilter;
        private EcsFilter _dirtyScaleFilter;
        private EcsPool<Position> _positions;
        private EcsPool<PositionWriteRequired> _positionWrites;
        private EcsPool<Rotation> _rotations;
        private EcsPool<RotationWriteRequired> _rotationWrites;
        private EcsPool<Scale> _scales;
        private EcsPool<ScaleWriteRequired> _scaleWrites;
        private EcsPool<UnityObjectData<Transform>> _transforms;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _dirtyPositionsFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Position>().Inc<PositionWriteRequired>()
                .End();
            _dirtyRotationsFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Rotation>().Inc<RotationWriteRequired>()
                .End();
            _dirtyScaleFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Scale>().Inc<ScaleWriteRequired>()
                .End();

            _transforms = world.GetPool<UnityObjectData<Transform>>();
            _positions = world.GetPool<Position>();
            _rotations = world.GetPool<Rotation>();
            _scales = world.GetPool<Scale>();
            _positionWrites = world.GetPool<PositionWriteRequired>();
            _rotationWrites = world.GetPool<RotationWriteRequired>();
            _scaleWrites = world.GetPool<ScaleWriteRequired>();
        }

        public void Run(EcsSystems systems)
        {
            WritePositions();
            WriteRotations();
            WriteScale();
        }

        private void WritePositions()
        {
            foreach (var i in _dirtyPositionsFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var position = ref _positions.Get(i);
                transform.position = position.WorldPosition;
                _positionWrites.Del(i);
            }
        }

        private void WriteRotations()
        {
            foreach (var i in _dirtyRotationsFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var rotation = ref _rotations.Get(i);
                transform.rotation = rotation.WorldRotation;
                _rotationWrites.Del(i);
            }
        }

        private void WriteScale()
        {
            foreach (var i in _dirtyScaleFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var scale = ref _scales.Get(i);
                transform.localScale = scale.LocalScale;
                _scaleWrites.Del(i);
            }
        }
    }
}
#endif