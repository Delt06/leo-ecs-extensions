using UnityEngine;

namespace Runner.Scripts
{
    [CreateAssetMenu(menuName = "Runner/Static Data")]
    public class StaticData : ScriptableObject
    {
        [SerializeField] private float _controlsSensitivity = 1f;
        [SerializeField] [Min(0f)] private float _levelHalfWidth = 1.5f;

        public float ControlsSensitivity => _controlsSensitivity;

        public float LevelHalfWidth => _levelHalfWidth;
    }
}