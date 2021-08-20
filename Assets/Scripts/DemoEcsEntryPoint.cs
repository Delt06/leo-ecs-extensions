using Cube;
using Cube.Presentation;
using DELTation.LeoEcsExtensions.Composition;
using Leopotam.Ecs;

public class DemoEcsEntryPoint : EcsEntryPoint
{
	protected override void PopulateSystems(EcsSystems systems, EcsWorld world)
	{
		// Construct a feature
		systems.StartBuildingFeature("Demo Feature")
			.Add(new StartSystem())
			.Add(new FramePrintingSystem())
			.Build()
			;

		// Or use a prebuilt one
		var colorPresenter = new CubeColorPresenter(world);
		systems.AddFeature(new AnimatedCubeFeature(colorPresenter));
	}
}