using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SidePositionSmoothSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly StaticData _staticData;

        public SidePositionSmoothSystem(StaticData staticData) => _staticData = staticData;

        public void Run(EcsSystems systems)
        {
            var filter = Filter<SidePosition>().End();

            foreach (var i in filter)
            {
                ref var sidePosition = ref Get<SidePosition>(i);
                sidePosition.CurrentPosition = Mathf.SmoothDamp(sidePosition.CurrentPosition,
                    sidePosition.TargetPosition, ref sidePosition.CurrentVelocity, _staticData.SideMovementSmoothTime,
                    float.PositiveInfinity, Time.deltaTime
                );
            }
        }
    }
}