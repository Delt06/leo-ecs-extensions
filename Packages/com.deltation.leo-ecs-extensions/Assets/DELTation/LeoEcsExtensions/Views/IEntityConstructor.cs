using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Views
{
	public interface IEntityInitializer
	{
		void InitializeEntity(EcsEntity entity);
	}
}