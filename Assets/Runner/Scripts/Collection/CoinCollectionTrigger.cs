using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Scripting;

namespace Runner.Collection
{
    public class CoinCollectionTrigger : MonoBehaviour
    {
        private EcsWorld _world;

        [Preserve]
        public void Construct(EcsWorld world)
        {
            _world = world;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CoinTag _)) return;

            ref var command = ref _world.NewPackedEntityWithWorld().Add<CollectCoinCommand>();
            command.CoinGameObject = other.gameObject;
        }
    }
}