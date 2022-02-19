using Cinemachine;
using UnityEngine;

namespace Runner._Shared
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Transform _levelSpawnPoint;
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;

        public Transform LevelSpawnPoint => _levelSpawnPoint;

        public Camera Camera => _camera;

        public CinemachineVirtualCamera PlayerVirtualCamera => _playerVirtualCamera;
    }
}