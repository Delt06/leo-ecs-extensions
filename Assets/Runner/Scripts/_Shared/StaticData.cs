using UnityEngine;

namespace Runner._Shared
{
    [CreateAssetMenu(menuName = "Runner/Static Data")]
    public class StaticData : ScriptableObject
    {
        [SerializeField] private float _controlsSensitivity = 1f;
        [SerializeField] [Min(0f)] private float _levelHalfWidth = 1.5f;
        [SerializeField] [Min(0f)] private float _movementSpeed = 1f;
        [SerializeField] [Min(0f)] private float _sideMovementSmoothTime = 0.1f;

        public float ControlsSensitivity => _controlsSensitivity;

        public float LevelHalfWidth => _levelHalfWidth;

        public float MovementSpeed => _movementSpeed;

        public float SideMovementSmoothTime => _sideMovementSmoothTime;
    }
}