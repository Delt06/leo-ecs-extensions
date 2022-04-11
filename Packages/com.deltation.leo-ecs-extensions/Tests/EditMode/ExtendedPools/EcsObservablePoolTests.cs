using DELTation.LeoEcsExtensions.ExtendedPools;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.ExtendedPools
{
    public class EcsObservablePoolTests
    {
        [Test]
        public void GivenObservablePool_WhenEntityNotPresent_ThenHasIsFalse()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();

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
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            var has = observablePool.Has(newEntity);

            // Assert
            Assert.That(has);
        }

        [Test]
        public void GivenObservablePool_WhenAddingComponent_ThenModified()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();
            var updatesPool = world.GetUpdatesPool<int>();

            // Act
            var newEntity = world.NewEntity();
            observablePool.Add(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
            Assert.That(updatesPool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenEmptyAndDeletingComponent_ThenNothingHappens()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();
            var updatesPool = world.GetUpdatesPool<int>();

            // Act
            var newEntity = world.NewEntity();
            observablePool.Del(newEntity);

            // Assert
            Assert.That(!pool.Has(newEntity));
            Assert.That(!updatesPool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndDeletingComponent_ThenDeletedAndModified()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();
            var updatesPool = world.GetUpdatesPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            observablePool.Del(newEntity);

            // Assert
            Assert.That(!pool.Has(newEntity));
            Assert.That(updatesPool.Has(newEntity));
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndModify_ThenModified()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();
            var updatesPool = world.GetUpdatesPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            observablePool.Modify(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
            Assert.That(updatesPool.Has(newEntity));
        }

        [Test]
        [DebugTest]
        public void GivenObservablePool_WhenNotPresentAndModify_ThenThrowsArgumentException()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();

            // Act
            var newEntity = world.NewEntity();
            void Code() => observablePool.Modify(newEntity);

            // Assert
            Assert.That(Code, Throws.ArgumentException);
        }

        [Test]
        public void GivenObservablePool_WhenPresentAndRead_ThenNotModified()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();
            var pool = world.GetPool<int>();
            var updatesPool = world.GetUpdatesPool<int>();

            // Act
            var newEntity = world.NewEntity();
            pool.Add(newEntity);
            observablePool.Read(newEntity);

            // Assert
            Assert.That(pool.Has(newEntity));
            Assert.That(!updatesPool.Has(newEntity));
        }

        [Test]
        [DebugTest]
        public void GivenObservablePool_WhenNotPresentAndRead_ThenThrowsArgumentException()
        {
            // Arrange
            var world = new EcsWorld();
            var observablePool = world.GetObservablePool<int>();

            // Act
            var newEntity = world.NewEntity();
            void Code() => observablePool.Read(newEntity);

            // Assert
            Assert.That(Code, Throws.ArgumentException);
        }
    }
}