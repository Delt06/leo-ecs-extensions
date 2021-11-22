using System;
using JetBrains.Annotations;
using NUnit.Framework;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Testing
{
    [TestFixture]
    public abstract class EcsTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            World = new EcsWorld();
            Systems = new EcsSystems(World);
            OnSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            Systems?.Destroy();
            World?.Destroy();
            OnTearDown();
        }

        protected EcsWorld World { get; private set; }
        protected EcsSystems Systems { get; private set; }

        protected virtual void OnSetUp() { }

        protected virtual void OnTearDown() { }

        protected void AddSystemAndInit([NotNull] IEcsSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            Systems.Add(system);
#if !LEOECS_EXTENSIONS_LITE
            Systems.ProcessInjects();
#endif
            Systems.Init();
        }
    }
}