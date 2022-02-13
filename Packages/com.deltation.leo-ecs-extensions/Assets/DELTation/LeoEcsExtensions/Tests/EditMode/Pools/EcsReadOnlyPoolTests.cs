using DELTation.LeoEcsExtensions.Pools;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.Pools
{
    public class EcsReadOnlyPoolTests
    {
        [Test]
        public void GivenObservablePool_WhenEntityNotPresent_ThenHasIsFalse()
        {
            // Arrange
            var world = new EcsWorld();
            var readOnlyPool = world.GetReadOnlyPool<int>();

            // Act
            var newEntity = world.NewEntity();
            var has = readOnlyPool.Has(newEntity);

            // Assert
            Assert.That(has, Is.False);
        }

        [Test]
        public void GivenObservablePool_WhenEntityIsPresent_ThenHasIsTrue()
        {
            // Arrange
            var world = new EcsWorld();
            var readOnlyPool = world.GetReadOnlyPool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            var has = readOnlyPool.Has(newEntity);

            // Assert
            Assert.That(has);
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndRead_ThenNotModified()
        {
            // Arrange
            var world = new EcsWorld();
            var readOnlyPool = world.GetReadOnlyPool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            readOnlyPool.Read(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
        }

        [Test]
        [DebugTest]
        public void GivenObservablePool_WhenNotPresentAndRead_ThenThrowsArgumentException()
        {
            // Arrange
            var world = new EcsWorld();
            var readOnlyPool = world.GetReadOnlyPool<int>();

            // Act
            var newEntity = world.NewEntity();
            void Code() => readOnlyPool.Read(newEntity);

            // Assert
            Assert.That(Code, Throws.ArgumentException);
        }
    }
}