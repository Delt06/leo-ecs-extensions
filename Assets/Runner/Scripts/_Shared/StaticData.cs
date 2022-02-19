using DELTation.LeoEcsExtensions.Views;
using Runner.Levels;
using UnityEngine;

namespace Runner._Shared
{
    [CreateAssetMenu(menuName = "Runner/Static Data")]
    public class StaticData : ScriptableObject
    {
        [SerializeField] private Level _levelPrefab;
        [SerializeField] private EntityView _playerPrefab;
        [SerializeField] private float _controlsSensitivity = 1f;
        [SerializeField] [Min(0f)] private float _levelHalfWidth = 1.5f;
        [SerializeField] [Min(0f)] private float _movementSpeed = 1f;
        [SerializeField] [Min(0f)] private float _sideMovementSmoothTime = 0.1f;

        public Level LevelPrefab => _levelPrefab;

        public float ControlsSensitivity => _controlsSensitivity;

        public float LevelHalfWidth => _levelHalfWidth;

        public float MovementSpeed => _movementSpeed;

        public float SideMovementSmoothTime => _sideMovementSmoothTime;

        public EntityView PlayerPrefab => _playerPrefab;
    }
}