using JetBrains.Annotations;
using Leopotam.EcsLite;
using Performance.Systems;

namespace Performance.Tests
{
    [UsedImplicitly]
    public class PackedFilterPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new PackedBenchmarkSystem(world);
    }
}