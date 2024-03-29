﻿using JetBrains.Annotations;
using Leopotam.EcsLite;
using Performance.Systems;

namespace Performance.Tests
{
    [UsedImplicitly]
    public class LiteFilterPerformanceTest : FilterPerformanceTestBase
    {
        protected override IEcsSystem CreateSystem(EcsWorld world) => new LiteBenchmarkSystem(world);
    }
}