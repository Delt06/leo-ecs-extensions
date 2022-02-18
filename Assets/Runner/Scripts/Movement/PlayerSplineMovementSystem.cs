using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;
using Runner._Shared.Utils;
using UnityEngine;

namespace Runner.Movement
{
    public class PlayerSplineMovementSystem : EcsRunSystemBase
    {
        private readonly Spline _spline;
        private readonly StaticData _staticData;

        public PlayerSplineMovementSystem(Spline spline, StaticData staticData)
        {
            _spline = spline;
            _staticData = staticData;
        }

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PlayerData> playerData,
            EcsPool<UnityObjectData<Transform>> transforms)
        {
            foreach (var i in filter)
            {
                var transform = transforms.Get(i).Object;
                ref var data = ref playerData.Get(i);

                var deltaDistance = _staticData.MovementSpeed * Time.deltaTime;
                (transform.position, data.T) = _spline.SamplePoint(data.T, deltaDistance);

                var forward = _spline.SampleDerivative(data.T).normalized;
                transform.forward = forward;
                data.Forward = forward;
            }
        }
    }
}