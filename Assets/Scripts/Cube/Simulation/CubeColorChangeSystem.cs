using Cube.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeColorChangeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CubeColorChangeCommand> _filter = default;


        public void Run()
        {
            foreach (var i in _filter)
            {
                var renderer = _filter.Get1(i).Cube.GetComponent<Renderer>();
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}