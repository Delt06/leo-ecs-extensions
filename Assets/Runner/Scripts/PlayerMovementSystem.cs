using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Scripts
{
    public class PlayerMovementSystem : EcsRunSystemBase
    {
        private readonly Spline _spline;
        private readonly StaticData _staticData;

        public PlayerMovementSystem(Spline spline, StaticData staticData)
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

                var deltaDistance = data.Speed * Time.deltaTime;
                Vector3 position;
                (position, data.T) = _spline.SamplePoint(data.T, deltaDistance);

                var forward = _spline.SampleDerivative(data.T).normalized;
                var right = Vector3.Cross(Vector3.up, forward);
                position += right * data.SidePositionNormalized * _staticData.LevelHalfWidth;
                transform.forward = forward;
                transform.position = position;
            }
        }
    }
}