using UnityEngine;

namespace Runner._Shared
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
    }
}