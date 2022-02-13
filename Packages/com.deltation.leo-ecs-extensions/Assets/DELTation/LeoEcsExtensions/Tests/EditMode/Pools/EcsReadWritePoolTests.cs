using DELTation.LeoEcsExtensions.Pools;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.Pools
{
    public class EcsReadWritePoolTests
    {
        [Test]
        public void GivenObservablePool_WhenEmptyAndDeletingComponent_ThenNothingHappens()
        {
            // Arrange
            var world = new EcsWorld();
            var readWritePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            readWritePool.Del(newEntity);

            // Assert
            Assert.That(!pool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndDeletingComponent_ThenDeleted()
        {
            // Arrange
            var world = new EcsWorld();
            var readWritePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            EcsTestUtils.PreventAutomaticDeletion(world, newEntity);
            pool.Add(newEntity);
            readWritePool.Del(newEntity);

            // Assert
            Assert.That(!pool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenEntityNotPresent_ThenHasIsFalse()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetReadWritePool<int>();

            // Act
            var newEntity = world.NewEntity();
            var has = observablePool.Has(newEntity);

            // Assert
            Assert.That(has, Is.False);
        }

        [Test]
        public void GivenObservablePool_WhenEntityIsPresent_ThenHasIsTrue()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            var has = observablePool.Has(newEntity);

            // Assert
            Assert.That(has);
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndRead_ThenNotModified()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            observablePool.Read(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
        }

        [Test]
        [DebugTest]
        public void GivenObservablePool_WhenNotPresentAndRead_ThenThrowsArgumentException()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetReadWritePool<int>();

            // Act
            var newEntity = world.NewEntity();
            void Code() => observablePool.Read(newEntity);

            // Assert
            Assert.That(Code, Throws.ArgumentException);
        }

        [Test]
        public void GivenObservablePool_WhenAddingComponent_ThenHasIsTrue()
        {
            // Arrange
            var world = new EcsWorld();
            var readWritePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            readWritePool.Add(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenHasAndCallingGetOrAdd_ThenNothingHappens()
        {
            // Arrange
            var world = new EcsWorld();
            var readWritePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            readWritePool.Add(newEntity);
            readWritePool.GetOrAdd(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenDoesNotHaveAndCallingGetOrAdd_ThenAdded()
        {
            // Arrange
            var world = new EcsWorld();
            var readWritePool = world.GetReadWritePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            readWritePool.GetOrAdd(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
        }
    }
}