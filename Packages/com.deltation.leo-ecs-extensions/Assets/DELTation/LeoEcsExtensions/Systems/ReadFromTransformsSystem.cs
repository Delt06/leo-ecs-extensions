using DELTation.LeoEcsExtensions.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Systems
{
	public sealed class ReadFromTransformsSystem : IEcsRunSystem
	{
		private readonly EcsFilter<UnityObjectData<Transform>, Position, PositionReadRequired> _dirtyPositionsFilter =
			default;
		private readonly EcsFilter<UnityObjectData<Transform>, Rotation, RotationReadRequired> _dirtyRotationsFilter =
			default;
		private readonly EcsFilter<UnityObjectData<Transform>, Scale, ScaleReadRequired> _dirtyScaleFilter =
			default;

		public void Run()
		{
			ReadPositions();
			ReadRotations();
			ReadScale();
		}

		private void ReadPositions()
		{
			foreach (var i in _dirtyPositionsFilter)
			{
				Transform transform = _dirtyPositionsFilter.Get1(i);
				ref var position = ref _dirtyPositionsFilter.Get2(i);
				position.WorldPosition = transform.position;
				_dirtyPositionsFilter.GetEntity(i).Del<PositionReadRequired>();
			}
		}

		private void ReadRotations()
		{
			foreach (var i in _dirtyRotationsFilter)
			{
				Transform transform = _dirtyRotationsFilter.Get1(i);
				ref var rotation = ref _dirtyRotationsFilter.Get2(i);
				rotation.WorldRotation = transform.rotation;
				_dirtyRotationsFilter.GetEntity(i).Del<RotationReadRequired>();
			}
		}

		private void ReadScale()
		{
			foreach (var i in _dirtyScaleFilter)
			{
				Transform transform = _dirtyScaleFilter.Get1(i);
				ref var scale = ref _dirtyScaleFilter.Get2(i);
				scale.LocalScale = transform.localScale;
				_dirtyScaleFilter.GetEntity(i).Del<ScaleReadRequired>();
			}
		}
	}
}