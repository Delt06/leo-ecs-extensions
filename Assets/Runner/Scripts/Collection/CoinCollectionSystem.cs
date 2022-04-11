using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Collection
{
    public class CoinCollectionSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly ICoinsService _coinsService;

        public CoinCollectionSystem(ICoinsService coinsService) => _coinsService = coinsService;

        public void Run(EcsSystems systems)
        {
            var filter = Filter<CollectCoinCommand>().End();
            foreach (var i in filter)
            {
                var collectCoinCommand = Get<CollectCoinCommand>(i);
                var coinGameObject = collectCoinCommand.CoinGameObject;
                if (coinGameObject == null) continue;

                _coinsService.Add(1);
                Object.Destroy(coinGameObject);
            }
        }
    }
}