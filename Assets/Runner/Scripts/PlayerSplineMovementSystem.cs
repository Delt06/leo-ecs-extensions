using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Scripts
{
    public class PlayerSplineMovementSystem : EcsRunSystemBase
    {
        private readonly Spline _spline;

        public PlayerSplineMovementSystem(Spline spline) => _spline = spline;

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PlayerData> playerData,
            EcsPool<UnityObjectData<Transform>> transforms)
        {
            foreach (var i in filter)
            {
                var transform = transforms.Get(i).Object;
                ref var data = ref playerData.Get(i);

                var deltaDistance = data.Speed * Time.deltaTime;
                (transform.position, data.T) = _spline.SamplePoint(data.T, deltaDistance);

                var forward = _spline.SampleDerivative(data.T).normalized;
                transform.forward = forward;
                data.Forward = forward;
            }
        }
    }
}