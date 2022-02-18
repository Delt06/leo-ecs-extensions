using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Collection
{
    public class CoinCollectionSystem : EcsRunSystemBase
    {
        private readonly ICoinsService _coinsService;

        public CoinCollectionSystem(ICoinsService coinsService) => _coinsService = coinsService;

        [EcsRun]
        private void Run(EcsFilter filter, EcsPool<CollectCoinCommand> commands)
        {
            foreach (var i in filter)
            {
                var collectCoinCommand = commands.Get(i);
                var coinGameObject = collectCoinCommand.CoinGameObject;
                if (coinGameObject == null) continue;

                _coinsService.Add(1);
                Object.Destroy(coinGameObject);
            }
        }
    }
}