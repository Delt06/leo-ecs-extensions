using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner.Movement;

namespace Runner.Input
{
    public class PointerDownHandlingSystem : EcsSystemBase, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = Filter<PointerDownEvent>().End();
            var sidePositions = World.GetSingletonPool<SidePosition>();
            var dragData = World.GetSingletonPool<ActiveDragData>();

            foreach (var i in filter)
            {
                var pointerDownEvent = Get<PointerDownEvent>(i);
                ref var activeDragData = ref dragData.Get();
                activeDragData.PointerPositionOnStart = pointerDownEvent.Position;
                ref var sidePosition = ref sidePositions.Get();
                activeDragData.SidePositionNormalizedOnStart = sidePosition.TargetPosition;

                GetOrAdd<CanMoveTag>(sidePositions.GetEntity());
            }
        }
    }
}