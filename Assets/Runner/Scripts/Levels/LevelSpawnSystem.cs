using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Runner._Shared;
using UnityEngine;

namespace Runner.Levels
{
    public class LevelSpawnSystem : EcsSystemBase, IEcsInitSystem
    {
        private readonly RuntimeData _runtimeData;
        private readonly SceneData _sceneData;
        private readonly StaticData _staticData;

        public LevelSpawnSystem(StaticData staticData, SceneData sceneData, RuntimeData runtimeData)
        {
            _staticData = staticData;
            _sceneData = sceneData;
            _runtimeData = runtimeData;
        }

        public void Init(EcsSystems systems)
        {
            var levelPrefab = _staticData.LevelPrefab;
            var levelSpawnPoint = _sceneData.LevelSpawnPoint;
            var level = Object.Instantiate(levelPrefab, levelSpawnPoint.position, levelSpawnPoint.rotation);
            _runtimeData.OnSpawnedLevel(level);
        }
    }
}