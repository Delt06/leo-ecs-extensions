using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SplineMovementSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        public SplineMovementSystem(RuntimeData runtimeData, StaticData staticData)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
        }

        public void Run(EcsSystems systems)
        {
            var spline = _runtimeData.Level.Spline;
            var filter = Filter<CanMoveTag>().Inc<SplineMovementData>().IncTransform().End();

            foreach (var i in filter)
            {
                var transform = GetTransform(i);
                ref var data = ref Get<SplineMovementData>(i);

                var deltaDistance = _staticData.MovementSpeed * Time.deltaTime;
                (transform.position, data.T) = spline.SamplePoint(data.T, deltaDistance);

                var forward = spline.SampleDerivative(data.T).normalized;
                transform.forward = forward;
                data.Forward = forward;
            }
        }
    }
}