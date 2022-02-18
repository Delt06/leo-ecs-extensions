using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using DELTation.LeoEcsExtensions.Composition.Di;
using UnityEngine;

namespace Runner.Scripts
{
    public class RunnerDiCompositionRoot : DependencyContainerBase
    {
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private StaticData _staticData;
        [SerializeField] private Spline _spline;
        [SerializeField] private InputSurface _inputSurface;

        protected override void ComposeDependencies(ICanRegisterContainerBuilder builder)
        {
            builder
                .Register(_spline)
                .Register(_sceneData)
                .Register(_staticData)
                .RegisterEcsEntryPoint<RunnerEcsEntryPoint>()
                .AttachEcsEntryPointViewTo(gameObject)
                ;

            builder
                .Register(_inputSurface).AsInternal()
                .Register<InputSurfacePresenter>()
                ;
        }
    }
}