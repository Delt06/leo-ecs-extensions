using UnityEngine;

namespace Runner.Scripts
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
    }
}