using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;

namespace Runner.Movement
{
    public class SplineMovementFinishSystem : EcsSystemBase
    {
        private readonly RuntimeData _runtimeData;

        public SplineMovementFinishSystem(RuntimeData runtimeData) => _runtimeData = runtimeData;

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<SplineMovementData> movementData, EcsPool<CanMoveTag> canMoveTags)
        {
            var spline = _runtimeData.Level.Spline;

            foreach (var i in filter)
            {
                ref var splineMovementData = ref movementData.Get(i);
                if (splineMovementData.T < spline.MaxT) continue;

                canMoveTags.Del(i);
            }
        }
    }
}