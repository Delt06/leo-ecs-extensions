using Leopotam.Ecs;
using UnityEngine;

namespace Cube
{
	public class CubeTranslationSystem : IEcsRunSystem
	{
		private readonly EcsFilter<CubeData> _cubeFilter = default;

		public void Run()
		{
			foreach (var i in _cubeFilter)
			{
				ref var cubeData = ref _cubeFilter.Get1(i);
				var cubeTransform = cubeData.Cube.transform;
				var cubePosition = cubeTransform.position;
				cubePosition.y = Mathf.Sin(Time.time);
				cubeTransform.position = cubePosition;
			}
		}
	}
}