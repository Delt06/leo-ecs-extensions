using JetBrains.Annotations;
using Leopotam.EcsLite;
using Performance.Systems;

namespace Performance.Tests
{
    [UsedImplicitly]
    public class LiteFilterAccessPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new LiteFilterAccessBenchmarkSystem(world);
    }
}