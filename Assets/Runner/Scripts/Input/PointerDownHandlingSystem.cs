using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Runner.Movement;

namespace Runner.Input
{
    public class PointerDownHandlingSystem : EcsSystemBase
    {
        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PointerDownEvent> pointerDownEvents,
            EcsSingletonPool<SidePosition> sidePositions,
            EcsSingletonPool<ActiveDragData> dragData,
            [EcsIgnore] EcsPool<CanMoveTag> canMoveTags)
        {
            foreach (var i in filter)
            {
                var pointerDownEvent = pointerDownEvents.Get(i);
                ref var activeDragData = ref dragData.Get();
                activeDragData.PointerPositionOnStart = pointerDownEvent.Position;
                ref var sidePosition = ref sidePositions.Get();
                activeDragData.SidePositionNormalizedOnStart = sidePosition.TargetPosition;

                canMoveTags.GetOrAdd(sidePositions.GetEntity());
            }
        }
    }
}