using Cube;
using Cube.Presentation;
using DELTation.LeoEcsExtensions.Composition;
using DELTation.LeoEcsExtensions.Composition.Di;
using Leopotam.EcsLite;
using Performance;

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

        systems
            .CreateAndAdd<TestComponentSystem>()
            .CreateAndAdd<TestComponentTextSystem>()
            .OneFrameUpdateEvents()
            ;

        systems
            .Add(new InitBenchmarkSystem(systems.GetWorld(), 1000))
            .CreateAndAdd<LiteBenchmarkSystem>()
            .CreateAndAdd<PackedBenchmarkSystem>()
            ;
    }
}