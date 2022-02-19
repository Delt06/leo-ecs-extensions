using DELTation.DIFramework.Lifecycle;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Runner.Movement;
using UnityEngine;

namespace Runner.Input
{
    public class InputSurfacePresenter : IStartable, IDestroyable
    {
        private readonly InputSurface _inputSurface;
        private readonly EcsWorld _world;

        public InputSurfacePresenter(InputSurface inputSurface, EcsWorld world)
        {
            _world = world;
            _inputSurface = inputSurface;
            world.Filter<SidePosition>().End();
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
            _world.NewPackedEntityWithWorld()
                .Add<PointerDragEvent>().Position = position;
        }

        private void OnPointerDown(Vector2 position)
        {
            _world.NewPackedEntityWithWorld()
                .Add<PointerDownEvent>().Position = position;
        }
    }
}