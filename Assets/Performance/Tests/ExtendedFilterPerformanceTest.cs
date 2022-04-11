using JetBrains.Annotations;
using Leopotam.EcsLite;
using Performance.Systems;

namespace Performance.Tests
{
    [UsedImplicitly]
    public class ExtendedFilterPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new ExtendedBenchmarkSystem(world);
    }
}