using DELTation.LeoEcsExtensions.Composition.Di;
using Runner.Movement;

namespace Runner
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