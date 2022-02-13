using Cube.Components;
using DELTation.LeoEcsExtensions.Utilities;
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

            cubeEntity.Add<CubeTag>();
            cubeEntity.Add<Position>();
            cubeEntity.OnUpdated<Position>();
            cubeEntity.Add<Rotation>();
            cubeEntity.OnUpdated<Rotation>();
        }
    }
}