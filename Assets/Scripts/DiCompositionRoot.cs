using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using DELTation.LeoEcsExtensions.Composition.Di;

public class DiCompositionRoot : DependencyContainerBase
{
    protected override void ComposeDependencies(ICanRegisterContainerBuilder builder)
    {
        builder
            .RegisterEcsEntryPoint<DemoEcsEntryPoint>()
            .AttachEcsEntryPointViewTo(gameObject)
            ;
    }
}