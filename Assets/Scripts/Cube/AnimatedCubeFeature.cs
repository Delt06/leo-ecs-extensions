using Cube.Components;
using Cube.Presentation;
using Cube.Simulation;
using Cube.View;
using DELTation.LeoEcsExtensions.Composition;
using DELTation.LeoEcsExtensions.Composition.Di;

namespace Cube
{
    public class AnimatedCubeFeature : PrebuiltFeature
    {
        private readonly CubeColorPresenter _cubeColorPresenter;

        public AnimatedCubeFeature(CubeColorPresenter cubeColorPresenter) => _cubeColorPresenter = cubeColorPresenter;

        protected override void ConfigureBuilder(EcsFeatureBuilder featureBuilder)
        {
            // input from view
            featureBuilder
                .Add(new CubeColorViewSystem(_cubeColorPresenter))
                ;

            // simulation
            featureBuilder
                .Add(new CubeSpawnSystem())
                .CreateAndAdd<CubeTranslationSystem>()
                .CreateAndAdd<CubeRotationSystem>()
                .Add(new CubeColorChangeSystem())
                .CreateAndAdd<CubeSyncTransformSystem>()
                ;

            // cleanup temporary components
            featureBuilder
                .OneFrame<CubeColorChangeCommand>()
                ;

            featureBuilder.CreateAndAdd<CubePositionChangeDebugSystem>()
                ;
        }
    }
}