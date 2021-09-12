using DELTation.LeoEcsExtensions.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Systems
{
    public sealed class WriteToTransformsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnityObjectData<Transform>, Position, PositionWriteRequired> _dirtyPositionsFilter =
            default;
        private readonly EcsFilter<UnityObjectData<Transform>, Rotation, RotationWriteRequired> _dirtyRotationsFilter =
            default;
        private readonly EcsFilter<UnityObjectData<Transform>, Scale, ScaleWriteRequired> _dirtyScaleFilter =
            default;

        public void Run()
        {
            WritePositions();
            WriteRotations();
            WriteScale();
        }

        private void WritePositions()
        {
            foreach (var i in _dirtyPositionsFilter)
            {
                Transform transform = _dirtyPositionsFilter.Get1(i);
                ref var position = ref _dirtyPositionsFilter.Get2(i);
                transform.position = position.WorldPosition;
                _dirtyPositionsFilter.GetEntity(i).Del<PositionWriteRequired>();
            }
        }

        private void WriteRotations()
        {
            foreach (var i in _dirtyRotationsFilter)
            {
                Transform transform = _dirtyRotationsFilter.Get1(i);
                ref var rotation = ref _dirtyRotationsFilter.Get2(i);
                transform.rotation = rotation.WorldRotation;
                _dirtyRotationsFilter.GetEntity(i).Del<RotationWriteRequired>();
            }
        }

        private void WriteScale()
        {
            foreach (var i in _dirtyScaleFilter)
            {
                Transform transform = _dirtyScaleFilter.Get1(i);
                ref var scale = ref _dirtyScaleFilter.Get2(i);
                transform.localScale = scale.LocalScale;
                _dirtyScaleFilter.GetEntity(i).Del<ScaleWriteRequired>();
            }
        }
    }
}