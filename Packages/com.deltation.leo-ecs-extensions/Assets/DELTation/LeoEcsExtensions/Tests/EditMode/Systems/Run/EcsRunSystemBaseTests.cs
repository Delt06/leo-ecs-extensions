using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.Systems.Run
{
    public class EcsRunSystemBaseTests
    {
        [Test]
        public void GivenInit_WhenCalled_ThenCallsOnAfterPreInit()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var system = new TimesRunSystem();
            ecsSystems.Add(system);

            // Act
            ecsSystems.Init();

            // Assert
            Assert.That(system.OnAfterPreInitCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void GivenRun_WhenCalledAfterInit_ThenRunDelegateIsExecuted()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var system = new TimesRunSystem();
            ecsSystems.Add(system);

            // Act
            ecsSystems.Init();
            ecsSystems.Run();

            // Assert
            Assert.That(system.RunTimes, Is.EqualTo(1));
        }

        [Test]
        public void GivenRun_WhenBeforePreInit_ThenThrowsException()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var system = new TimesRunSystem();
            ecsSystems.Add(system);

            // Act
            void Code() => system.Run(null);

            // Assert
#if DEBUG
            Assert.That(Code, Throws.InvalidOperationException);
#else
            Assert.That(Code, Throws.InstanceOf<NullReferenceException>());
#endif
        }

        [Test]
        public void GivenRun_WhenAfterPreInit_ThenThrows()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var system = new TimesRunSystem();
            ecsSystems.Add(system);
            ecsSystems.Init();

            // Act
            void Code() => ecsSystems.Run();

            // Assert
            Assert.That(Code, Throws.Nothing);
        }

        [Test]
        public void GivenInit_WhenCalled_ThenInitializesBeforeOtherInits()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var queue = new Queue<object>();
            var runSystem = new PreInitOrderRunSystem(queue);
            var initSystem = new InitSystem(queue);
            ecsSystems.Add(initSystem);
            ecsSystems.Add(runSystem);

            // Act
            ecsSystems.Init();

            // Assert
            Assert.That(queue, Is.EquivalentTo(new object[] { runSystem, initSystem }));
        }

        [Test]
        public void GivenRun_WhenQueryingWorld_ThenItIsMainWorld()
        {
            // Arrange
            var world = new EcsWorld();
            var ecsSystems = new EcsSystems(world);
            var runSystem = new GetWorldSystem();
            ecsSystems.Add(runSystem);

            // Act
            ecsSystems.Init();
            ecsSystems.Run();

            // Assert
            Assert.That(runSystem.World, Is.EqualTo(world));
        }

        private class InitSystem : IEcsInitSystem
        {
            private readonly Queue<object> _queue;

            public InitSystem(Queue<object> queue) => _queue = queue;

            public void Init(EcsSystems systems)
            {
                _queue.Enqueue(this);
            }
        }

        private class PreInitOrderRunSystem : EcsRunSystemBase
        {
            private readonly Queue<object> _queue;

            public PreInitOrderRunSystem(Queue<object> queue) => _queue = queue;

            protected override void OnAfterPreInit(EcsSystems systems)
            {
                base.OnAfterPreInit(systems);
                _queue.Enqueue(this);
            }

            protected override EcsBuiltRunSystem Build(EcsWorld world) => world.Filter<int>().MapTo(() => { });
        }

        private class TimesRunSystem : EcsRunSystemBase
        {
            public int OnAfterPreInitCalledTimes { get; private set; }
            public int RunTimes { get; private set; }

            protected override void OnAfterPreInit(EcsSystems systems)
            {
                base.OnAfterPreInit(systems);
                OnAfterPreInitCalledTimes++;
            }

            protected override EcsBuiltRunSystem Build(EcsWorld world) => world.Filter<int>().MapTo(
                () => { RunTimes++; }
            );
        }

        private class GetWorldSystem : EcsRunSystemBase
        {
            public EcsWorld World { get; private set; }

            protected override EcsBuiltRunSystem Build(EcsWorld world) => world.Filter<int>().MapTo(
                (EcsWorld w) => { World = w; }
            );
        }
    }
}