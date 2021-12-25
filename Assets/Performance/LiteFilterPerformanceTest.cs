using Leopotam.EcsLite;

namespace Performance
{
    public class LiteFilterPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new LiteBenchmarkSystem(world);
    }
}