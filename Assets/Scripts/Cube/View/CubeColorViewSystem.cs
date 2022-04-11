using Cube.Presentation;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.View
{
    public sealed class CubeColorViewSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly CubeColorPresenter _cubeColorPresenter;
        private Camera _camera;

        public CubeColorViewSystem(CubeColorPresenter cubeColorPresenter) => _cubeColorPresenter = cubeColorPresenter;

        public void Init(EcsSystems systems)
        {
            _camera = Camera.main;
        }

        public void Run(EcsSystems systems)
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider.gameObject.name != "Cube") return;

            _cubeColorPresenter.OnClicked(hit.collider.gameObject);
        }
    }
}