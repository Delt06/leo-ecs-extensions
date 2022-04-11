using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Performance.Components;
using UnityEngine;

namespace Performance.Systems
{
    public class SystemBaseBenchmarkSystem : EcsSystemBase, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            foreach (var i in World.Filter<PositionsComponent>().End())
            {
                ref readonly var position = ref Read<PositionsComponent>(i);
                var direction = Vector3.ClampMagnitude(position.P1 - position.P2, 1f);
                GetOrAdd<DirectionComponent>(i).Direction = direction;
            }
        }
    }
}