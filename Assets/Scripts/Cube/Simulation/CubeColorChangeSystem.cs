using Cube.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeColorChangeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsReadOnlyPool<CubeColorChangeCommand> _commands;
        private EcsFilter _filter;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<CubeColorChangeCommand>().End();
            _commands = world.GetPool<CubeColorChangeCommand>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var renderer = _commands.Read(i).Cube.GetComponent<Renderer>();
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}