using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Movement
{
    public class SplineSideMovementSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly StaticData _staticData;

        public SplineSideMovementSystem(StaticData staticData) => _staticData = staticData;

        public void Run(EcsSystems systems)
        {
            var filter = Filter<CanMoveTag>().Inc<SplineMovementData>().Inc<SidePosition>().IncTransform().End();
            foreach (var i in filter)
            {
                var data = Get<SplineMovementData>(i);
                var transform = GetTransform(i);

                var right = Vector3.Cross(Vector3.up, data.Forward);
                var sidePosition = Get<SidePosition>(i).CurrentPosition;
                transform.position += sidePosition * _staticData.LevelHalfWidth
                                                   * right;
            }
        }
    }
}