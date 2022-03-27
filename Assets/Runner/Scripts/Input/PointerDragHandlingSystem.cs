using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner._Shared;
using Runner.Movement;
using UnityEngine;

namespace Runner.Input
{
    public class PointerDragHandlingSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly StaticData _staticData;

        public PointerDragHandlingSystem(SceneData sceneData, StaticData staticData)
        {
            _sceneData = sceneData;
            _staticData = staticData;
        }

        public void Run(EcsSystems systems)
        {
            var camera = _sceneData.Camera;
            var filter = Filter<PointerDragEvent>().End();
            var dragData = World.GetSingletonPool<ActiveDragData>();
            var sidePositions = World.GetSingletonPool<SidePosition>();

            foreach (var i in filter)
            {
                ref var pointerDragEvent = ref Get<PointerDragEvent>(i);
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