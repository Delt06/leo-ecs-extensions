using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class SystemBaseBenchmarkSystem : EcsSystemBase
    {
        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PositionsComponent> positions,
            [EcsIgnoreInc] EcsPool<DirectionComponent> directions)
        {
            foreach (var i in filter)
            {
                ref var position = ref positions.Get(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                directions.GetOrAdd(i).Direction = direction;
            }
        }
    }

    public class SystemBase2BenchmarkSystem : EcsSystemBase2, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            foreach (var i in World.Filter<PositionsComponent>().End())
            {
                ref var position = ref Get<PositionsComponent>(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                GetOrAdd<DirectionComponent>(i).Direction = direction;
            }
        }
    }
}