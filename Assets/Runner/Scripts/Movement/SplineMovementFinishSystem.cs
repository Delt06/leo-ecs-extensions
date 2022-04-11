using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner._Shared;

namespace Runner.Movement
{
    public class SplineMovementFinishSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly RuntimeData _runtimeData;

        public SplineMovementFinishSystem(RuntimeData runtimeData) => _runtimeData = runtimeData;

        public void Run(EcsSystems systems)
        {
            var spline = _runtimeData.Level.Spline;
            var filter = Filter<SplineMovementData>().Inc<CanMoveTag>().End();

            foreach (var i in filter)
            {
                ref var splineMovementData = ref Get<SplineMovementData>(i);
                if (splineMovementData.T < spline.MaxT) continue;

                Del<CanMoveTag>(i);
            }
        }
    }
}