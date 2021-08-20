using System;
using JetBrains.Annotations;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Features
{
	public class OneFrameFeature<T> : Feature where T : struct
	{
		public override void Register([NotNull] EcsSystems systems, [NotNull] EcsSystems physicsSystems)
		{
			if (systems == null) throw new ArgumentNullException(nameof(systems));
			if (physicsSystems == null) throw new ArgumentNullException(nameof(physicsSystems));
			systems.OneFrame<T>();
		}
	}
}