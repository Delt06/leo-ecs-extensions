using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class SystemBaseExtendedBenchmarkSystem : EcsSystemBase
    {
        [EcsRun]
        private void Run(EcsFilter filter, EcsReadWritePool<PositionsComponent> positions,
            [EcsIgnore] EcsReadWritePool<DirectionComponent> directions)
        {
            foreach (var i in filter)
            {
                ref var position = ref positions.Get(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                directions.GetOrAdd(i).Direction = direction;
            }
        }
    }
}