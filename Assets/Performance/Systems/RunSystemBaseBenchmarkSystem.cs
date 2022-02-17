using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class RunSystemBaseBenchmarkSystem : EcsRunSystemBase
    {
        protected override EcsBuiltRunSystem Build(EcsWorld world) =>
            world.Filter<PositionsComponent>().MapTo(
                (EcsFilter filter, EcsPool<PositionsComponent> positions, EcsPool<DirectionComponent> directions) =>
                {
                    foreach (var i in filter)
                    {
                        ref var position = ref positions.Get(i);
                        var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                        directions.GetOrAdd(i).Direction = direction;
                    }
                }
            );
    }
}