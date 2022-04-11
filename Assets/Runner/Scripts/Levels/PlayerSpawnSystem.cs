using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Levels
{
    public class PlayerSpawnSystem : EcsSystemBase, IEcsInitSystem
    {
        private readonly SceneData _sceneData;
        private readonly StaticData _staticData;

        public PlayerSpawnSystem(StaticData staticData, SceneData sceneData)
        {
            _staticData = staticData;
            _sceneData = sceneData;
        }

        public void Init(EcsSystems systems)
        {
            var playerPrefab = _staticData.PlayerPrefab;
            var levelSpawnPoint = _sceneData.LevelSpawnPoint;
            var player = Object.Instantiate(playerPrefab, levelSpawnPoint.position, levelSpawnPoint.rotation);

            InitializeCamera(player.transform);
        }

        private void InitializeCamera(Transform playerTransform)
        {
            var playerVirtualCamera = _sceneData.PlayerVirtualCamera;
            playerVirtualCamera.Follow = playerTransform;
            playerVirtualCamera.LookAt = playerTransform;
        }
    }
}