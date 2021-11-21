#if LEOECS_EXTENSIONS_LITE
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Systems
{
    public sealed class ReadFromTransformsSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _dirtyPositionsFilter;
        private EcsFilter _dirtyRotationsFilter;
        private EcsFilter _dirtyScaleFilter;
        private EcsPool<PositionReadRequired> _positionReads;
        private EcsPool<Position> _positions;
        private EcsPool<RotationReadRequired> _rotationReads;
        private EcsPool<Rotation> _rotations;
        private EcsPool<ScaleReadRequired> _scaleReads;
        private EcsPool<Scale> _scales;
        private EcsPool<UnityObjectData<Transform>> _transforms;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _dirtyPositionsFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Position>().Inc<PositionReadRequired>()
                .End();
            _dirtyRotationsFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Rotation>().Inc<RotationReadRequired>()
                .End();
            _dirtyScaleFilter = world
                .Filter<UnityObjectData<Transform>>().Inc<Scale>().Inc<ScaleReadRequired>()
                .End();

            _transforms = world.GetPool<UnityObjectData<Transform>>();
            _positions = world.GetPool<Position>();
            _rotations = world.GetPool<Rotation>();
            _scales = world.GetPool<Scale>();
            _positionReads = world.GetPool<PositionReadRequired>();
            _rotationReads = world.GetPool<RotationReadRequired>();
            _scaleReads = world.GetPool<ScaleReadRequired>();
        }

        public void Run(EcsSystems systems)
        {
            ReadPositions();
            ReadRotations();
            ReadScale();
        }

        private void ReadPositions()
        {
            foreach (var i in _dirtyPositionsFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var position = ref _positions.Get(i);
                position.WorldPosition = transform.position;
                _positionReads.Del(i);
            }
        }

        private void ReadRotations()
        {
            foreach (var i in _dirtyRotationsFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var rotation = ref _rotations.Get(i);
                rotation.WorldRotation = transform.rotation;
                _rotationReads.Del(i);
            }
        }

        private void ReadScale()
        {
            foreach (var i in _dirtyScaleFilter)
            {
                Transform transform = _transforms.Get(i);
                ref var scale = ref _scales.Get(i);
                scale.LocalScale = transform.localScale;
                _scaleReads.Del(i);
            }
        }
    }
}

#endif