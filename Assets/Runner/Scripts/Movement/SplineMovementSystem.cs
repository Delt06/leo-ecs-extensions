using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SplineMovementSystem : EcsSystemBase
    {
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        public SplineMovementSystem(RuntimeData runtimeData, StaticData staticData)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
        }

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<SplineMovementData> movementData,
            EcsPool<UnityObjectData<Transform>> transforms)
        {
            var spline = _runtimeData.Level.Spline;

            foreach (var i in filter)
            {
                var transform = transforms.Get(i).Object;
                ref var data = ref movementData.Get(i);

                var deltaDistance = _staticData.MovementSpeed * Time.deltaTime;
                (transform.position, data.T) = spline.SamplePoint(data.T, deltaDistance);

                var forward = spline.SampleDerivative(data.T).normalized;
                transform.forward = forward;
                data.Forward = forward;
            }
        }
    }
}