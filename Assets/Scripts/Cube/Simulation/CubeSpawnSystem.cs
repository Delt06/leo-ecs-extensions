using Cube.Components;
using DELTation.LeoEcsExtensions.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Cube.Simulation
{
	public class CubeSpawnSystem : IEcsInitSystem
	{
		private readonly EcsWorld _world = default;

		public void Init()
		{
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "Cube";

			var cubeEntity = _world.NewEntity();
			cubeEntity.SetUnityObjectData(cube.transform);
			cubeEntity.SetTransformComponentsFromTransform(cube.transform);

			cubeEntity.Get<CubeTag>();
		}
	}
}