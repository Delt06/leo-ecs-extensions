using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubeSpawnSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube";

            var cubeEntity = world.NewPackedEntityWithWorld();
            cubeEntity.SetUnityObjectData(cube.transform);
            cubeEntity.SetTransformComponentsFromTransform(cube.transform);

            cubeEntity.Add<CubeTag>();
        }
    }
}