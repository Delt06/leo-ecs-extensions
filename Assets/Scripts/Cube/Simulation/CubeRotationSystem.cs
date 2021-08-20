using Cube.Components;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.Ecs;
using UnityEngine;

namespace Cube.Simulation
{
	public class CubeRotationSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Rotation, CubeTag> _cubesFilter = default;

		public void Run()
		{
			foreach (var i in _cubesFilter)
			{
				ref var rotation = ref _cubesFilter.Get1(i);
				const float rotationSpeed = 90f;
				var newWorldRotation = Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up) *
				                       rotation.WorldRotation;
				_cubesFilter.GetEntity(i).UpdateRotation(ref rotation, newWorldRotation);
			}
		}
	}
}