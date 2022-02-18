using DELTation.LeoEcsExtensions.Composition.Di;

namespace Runner.Scripts
{
    public class RunnerEcsEntryPoint : EcsEntryPoint
    {
        public override void PopulateSystems(EcsFeatureBuilder featureBuilder)
        {
            featureBuilder
                .CreateAndAdd<PlayerSplineMovementSystem>()
                .CreateAndAdd<PlayerSideMovementSystem>()
                ;
        }
    }
}