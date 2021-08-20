using Leopotam.Ecs;
using UnityEngine;

namespace Cube
{
	public class CubeSpawnSystem : IEcsInitSystem
	{
		private readonly EcsWorld _world = default;

		public void Init()
		{
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "Cube";

			var cubeEntity = _world.NewEntity();
			ref var cubeData = ref cubeEntity.Get<CubeData>();
			cubeData.Cube = cube;
		}
	}
}