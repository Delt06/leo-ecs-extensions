using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.Ecs;
using UnityEngine;

namespace Cube.Simulation
{
	public class CubeTranslationSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Position, CubeTag> _cubeFilter = default;

		public void Run()
		{
			foreach (var i in _cubeFilter)
			{
				ref var position = ref _cubeFilter.Get1(i);
				var newWorldPosition = position.WorldPosition;
				newWorldPosition.y = Mathf.Sin(Time.time);
				_cubeFilter.GetEntity(i).UpdatePosition(ref position, newWorldPosition);
			}
		}
	}
}