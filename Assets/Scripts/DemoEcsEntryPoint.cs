using Cube;
using DELTation.LeoEcsExtensions.Composition;
using Leopotam.Ecs;

public class DemoEcsEntryPoint : EcsEntryPoint
{
	protected override void PopulateSystems(EcsSystems systems)
	{
		// Construct a feature
		systems.StartBuildingFeature("Demo Feature")
			.Add(new StartSystem())
			.Add(new FramePrintingSystem())
			.Build()
			;

		// Or use a prebuilt one
		systems.AddFeature(new AnimatedCubeFeature());
	}
}