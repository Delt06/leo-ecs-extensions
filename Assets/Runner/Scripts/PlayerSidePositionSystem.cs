using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Scripts
{
    public class PlayerSidePositionSystem : EcsRunSystemBase
    {
        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<PlayerData> playerData)
        {
            foreach (var i in filter)
            {
                ref var data = ref playerData.Get(i);
                data.SidePositionNormalized += Time.deltaTime * Input.GetAxis("Horizontal");
                data.SidePositionNormalized = Mathf.Clamp(data.SidePositionNormalized, -1, 1f);
            }
        }
    }
}