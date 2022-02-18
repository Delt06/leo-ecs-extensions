using DELTation.DIFramework.Lifecycle;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Runner.Scripts
{
    public class InputSurfacePresenter : IStartable, IDestroyable
    {
        private readonly EcsFilter _filter;
        private readonly InputSurface _inputSurface;
        private readonly SceneData _sceneData;
        private readonly StaticData _staticData;
        private DragData _dragData;

        public InputSurfacePresenter(InputSurface inputSurface, SceneData sceneData, EcsWorld world,
            StaticData staticData)
        {
            _inputSurface = inputSurface;
            _sceneData = sceneData;
            _staticData = staticData;
            _filter = world.Filter<PlayerData>().End();
        }

        public void OnDestroy()
        {
            _inputSurface.PointerDown -= OnPointerDown;
            _inputSurface.Drag -= OnDrag;
        }

        public void OnStart()
        {
            _inputSurface.PointerDown += OnPointerDown;
            _inputSurface.Drag += OnDrag;
        }

        private void OnDrag(Vector2 position)
        {
            var camera = _sceneData.Camera;
            var offset = position - _dragData.PointerPositionOnStart;
            var xOffsetViewport = camera.ScreenToViewportPoint(offset).x;
            var deltaSidePosition = xOffsetViewport * _staticData.ControlsSensitivity;

            foreach (var i in _filter)
            {
                ref var playerData = ref _filter.Get<PlayerData>(i);
                var sidePositionNormalized = _dragData.SidePositionNormalizedOnStart + deltaSidePosition;
                sidePositionNormalized = Mathf.Clamp(sidePositionNormalized, -1f, 1f);
                playerData.SidePositionNormalized = sidePositionNormalized;
            }
        }

        private void OnPointerDown(Vector2 position)
        {
            _dragData.PointerPositionOnStart = position;

            foreach (var i in _filter)
            {
                _dragData.SidePositionNormalizedOnStart = _filter.Get<PlayerData>(i).SidePositionNormalized;
                break;
            }
        }

        private struct DragData
        {
            public float SidePositionNormalizedOnStart;
            public Vector2 PointerPositionOnStart;
        }
    }
}