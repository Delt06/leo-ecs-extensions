using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Presentation
{
    public class CubeColorPresenter
    {
        private readonly EcsPool<CubeColorChangeCommand> _pool;
        private readonly EcsWorld _world;

        public CubeColorPresenter(EcsWorld world)
        {
            _world = world;
            _pool = _world.GetPool<CubeColorChangeCommand>();
        }

        public void OnClicked(GameObject cube)
        {
            _pool.GetOrAdd(_world.NewEntity()).Cube = cube;
        }
    }
}