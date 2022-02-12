using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using DELTation.LeoEcsExtensions.Composition.Di;
using UnityEngine;

public class DiCompositionRoot : DependencyContainerBase
{
    [SerializeField] private EcsEntryPoint _ecsEntryPoint;

    protected override void ComposeDependencies(ICanRegisterContainerBuilder builder)
    {
        builder
            .RegisterEcsEntryPoint(_ecsEntryPoint)
            ;
    }
}