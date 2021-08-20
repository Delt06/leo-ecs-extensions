using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Features
{
	public abstract class Feature
	{
		public abstract void Register(EcsSystems systems, EcsSystems physicsSystems);
	}
}