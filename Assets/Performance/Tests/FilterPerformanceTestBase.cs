using Leopotam.EcsLite;
using NUnit.Framework;
using Performance.Systems;
using Unity.PerformanceTesting;

namespace Performance.Tests
{
    public abstract class FilterPerformanceTestBase
    {
        private EcsSystems _systems;

        private EcsWorld _world;


        [Test] [Performance]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Explicit]
        public void Benchmark(int entities)
        {
            Measure.Method(() =>
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            _systems.Run();
                        }
                    }
                )
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(1)
                .SetUp(() =>
                    {
                        _world = new EcsWorld();
                        _systems = new EcsSystems(_world)
                                .Add(new InitBenchmarkSystem(_world, entities))
                                .Add(CreateSystem(_world))
                            ;
                        _systems.Init();
                    }
                )
                .CleanUp(() =>
                    {
                        _systems?.Destroy();
                        _world?.Destroy();
                    }
                )
                .Run();
        }

        protected abstract IEcsSystem CreateSystem(EcsWorld world);
    }
}