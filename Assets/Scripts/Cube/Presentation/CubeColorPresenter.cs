using Cube.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Cube.Presentation
{
    public class CubeColorPresenter
    {
        private readonly EcsWorld _world;

        public CubeColorPresenter(EcsWorld world) => _world = world;

        public void OnClicked(GameObject cube)
        {
            _world.NewEntity().Get<CubeColorChangeCommand>().Cube = cube;
        }
    }
}