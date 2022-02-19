using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SplineSideMovementSystem : EcsSystemBase
    {
        private readonly StaticData _staticData;

        public SplineSideMovementSystem(StaticData staticData) => _staticData = staticData;

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<SplineMovementData> movementData,
            EcsPool<SidePosition> sidePositions,
            EcsPool<UnityRef<Transform>> transforms)
        {
            foreach (var i in filter)
            {
                var data = movementData.Get(i);
                Transform transform = transforms.Get(i);

                var right = Vector3.Cross(Vector3.up, data.Forward);
                var sidePosition = sidePositions.Get(i).CurrentPosition;
                transform.position += right * sidePosition
                                            * _staticData.LevelHalfWidth;
            }
        }
    }
}