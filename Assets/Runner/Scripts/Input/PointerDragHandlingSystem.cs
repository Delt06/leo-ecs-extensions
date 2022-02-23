using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;
using Runner.Movement;
using UnityEngine;

namespace Runner.Input
{
    public class PointerDragHandlingSystem : EcsSystemBase
    {
        private readonly SceneData _sceneData;
        private readonly StaticData _staticData;

        public PointerDragHandlingSystem(SceneData sceneData, StaticData staticData)
        {
            _sceneData = sceneData;
            _staticData = staticData;
        }

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PointerDragEvent> pointerDragEvents,
            EcsSingletonPool<SidePosition> sidePositions,
            EcsSingletonPool<ActiveDragData> dragData,
            [EcsIgnoreInc] EcsPool<CanMoveTag> canMoveTags)
        {
            var camera = _sceneData.Camera;

            foreach (var i in filter)
            {
                ref var pointerDragEvent = ref pointerDragEvents.Get(i);
                ref var activeDragData = ref dragData.Get();
                var position = pointerDragEvent.Position;

                var offset = position - activeDragData.PointerPositionOnStart;
                var xOffsetViewport = camera.ScreenToViewportPoint(offset).x;
                var deltaSidePosition = xOffsetViewport * _staticData.ControlsSensitivity;

                ref var sidePosition = ref sidePositions.Get();
                var sidePositionNormalized = activeDragData.SidePositionNormalizedOnStart + deltaSidePosition;
                sidePositionNormalized = Mathf.Clamp(sidePositionNormalized, -1f, 1f);
                sidePosition.TargetPosition = sidePositionNormalized;
            }
        }
    }
}