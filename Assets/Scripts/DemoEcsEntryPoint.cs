using Cube;
using Cube.Presentation;
using DELTation.LeoEcsExtensions.Composition.Di;

public class DemoEcsEntryPoint : EcsEntryPoint
{
    public override void PopulateSystems(EcsFeatureBuilder featureBuilder)
    {
        featureBuilder
            .Add(new StartSystem())
            .Add(new FramePrintingSystem())
            ;

        // Use prebuilt feature
        var colorPresenter = new CubeColorPresenter(World);
        featureBuilder.AddFeature(new AnimatedCubeFeature(colorPresenter));

        featureBuilder
            .CreateAndAdd<TestComponentRunSystem>()
            .CreateAndAdd<TestComponentTextSystem>()
            .OneFrameUpdateEvents()
            ;
    }

    public override void PopulateLateSystems(EcsFeatureBuilder featureBuilder)
    {
        // Just for testing purposes
        featureBuilder.OneFrame<int>();
    }

    public override void PopulatePhysicsSystems(EcsFeatureBuilder featureBuilder)
    {
        // Just for testing purposes
        featureBuilder.OneFrame<float>();
    }
}