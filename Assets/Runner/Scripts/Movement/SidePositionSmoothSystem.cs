using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SidePositionSmoothSystem : EcsSystemBase
    {
        private readonly StaticData _staticData;

        public SidePositionSmoothSystem(StaticData staticData) => _staticData = staticData;

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<SidePosition> sidePositions)
        {
            foreach (var i in filter)
            {
                ref var sidePosition = ref sidePositions.Get(i);
                sidePosition.CurrentPosition = Mathf.SmoothDamp(sidePosition.CurrentPosition,
                    sidePosition.TargetPosition, ref sidePosition.CurrentVelocity, _staticData.SideMovementSmoothTime,
                    float.PositiveInfinity, Time.deltaTime
                );
            }
        }
    }
}