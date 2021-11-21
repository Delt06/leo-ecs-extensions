using Cube.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeColorChangeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsPool<CubeColorChangeCommand> _commands;
        private EcsFilter _filter;


        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<CubeColorChangeCommand>().End();
            _commands = world.GetPool<CubeColorChangeCommand>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var renderer = _commands.Get(i).Cube.GetComponent<Renderer>();
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}