using Leopotam.EcsLite;

namespace Performance
{
    public class PackedFilterPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new PackedBenchmarkSystem(world);
    }
}