using Leopotam.Ecs;
using UnityEngine;

namespace Cube
{
	public class CubeRotationSystem : IEcsRunSystem
	{
		private readonly EcsFilter<CubeData> _cubesFilter = default;

		public void Run()
		{
			foreach (var i in _cubesFilter)
			{
				ref var cubeData = ref _cubesFilter.Get1(i);
				const float rotationSpeed = 90f;
				cubeData.Cube.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
			}
		}
	}
}