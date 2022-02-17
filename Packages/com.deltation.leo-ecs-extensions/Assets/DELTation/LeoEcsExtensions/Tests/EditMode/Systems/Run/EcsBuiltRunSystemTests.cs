using System;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.Systems.Run
{
    public class EcsBuiltRunSystemTests
    {
        private EcsFilter _filter;
        private EcsWorld _world;

        [SetUp]
        public void SetUp()
        {
            _world = new EcsWorld();
            _filter = _world.Filter<int>().End();
        }

        [Test]
        public void GivenRun_WhenQueryingEcsFilter_ThenReturnsTheProvidedFilter()
        {
            // Arrange
            EcsFilter queriedFilter = null;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsFilter>(f => { queriedFilter = f; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedFilter, Is.Not.Null);
            Assume.That(queriedFilter, Is.EqualTo(_filter));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsWorld_ThenReturnsTheUsedWorld()
        {
            // Arrange
            EcsWorld queriedWorld = null;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsWorld>(w => { queriedWorld = w; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedWorld, Is.Not.Null);
            Assume.That(queriedWorld, Is.EqualTo(_world));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsPool_ThenReturnsCorrectPool()
        {
            // Arrange
            EcsPool<float> queriedPool = null;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsPool<float>>(p => { queriedPool = p; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedPool, Is.Not.Null);
            Assume.That(queriedPool, Is.EqualTo(_world.GetPool<float>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsReadOnlyPool_ThenReturnsCorrectPool()
        {
            // Arrange
            EcsReadOnlyPool<float> queriedPool = default;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsReadOnlyPool<float>>(
                    p => { queriedPool = p; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedPool, Is.Not.EqualTo(default(EcsReadOnlyPool<float>)));
            Assume.That(queriedPool, Is.EqualTo(_world.GetReadOnlyPool<float>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsReadWritePool_ThenReturnsCorrectPool()
        {
            // Arrange
            EcsReadWritePool<float> queriedPool = default;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsReadWritePool<float>>(
                    p => { queriedPool = p; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedPool, Is.Not.EqualTo(default(EcsReadWritePool<float>)));
            Assume.That(queriedPool, Is.EqualTo(_world.GetReadWritePool<float>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsObservablePool_ThenReturnsCorrectPool()
        {
            // Arrange
            EcsObservablePool<float> queriedPool = default;
            var builtRunSystem = new EcsBuiltRunSystem(_filter, new Action<EcsObservablePool<float>>(
                    p => { queriedPool = p; }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedPool, Is.Not.EqualTo(default(EcsObservablePool<float>)));
            Assume.That(queriedPool, Is.EqualTo(_world.GetObservablePool<float>()));
        }

        [Test]
        public void GivenRun_WhenQueryingMultipleObjects_ThenReturnsAll()
        {
            // Arrange
            EcsObservablePool<float> queriedPool = default;
            EcsWorld queriedWorld = null;
            EcsFilter queriedFilter = null;
            var builtRunSystem = new EcsBuiltRunSystem(_filter,
                new Action<EcsObservablePool<float>, EcsWorld, EcsFilter>(
                    (p, w, f) =>
                    {
                        queriedPool = p;
                        queriedWorld = w;
                        queriedFilter = f;
                    }
                )
            );

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(queriedPool, Is.Not.EqualTo(default(EcsObservablePool<float>)));
            Assume.That(queriedPool, Is.EqualTo(_world.GetObservablePool<float>()));

            Assert.That(queriedWorld, Is.Not.Null);
            Assume.That(queriedWorld, Is.EqualTo(_world));

            Assert.That(queriedFilter, Is.Not.Null);
            Assume.That(queriedFilter, Is.EqualTo(_filter));
        }

        [Test]
        public void GivenRun_WhenQueryingUnsupportedType_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                // ReSharper disable once ObjectCreationAsStatement
                new EcsBuiltRunSystem(_filter, new Action<int>(i => { }));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithInParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                // ReSharper disable once ObjectCreationAsStatement
                new EcsBuiltRunSystem(_filter, new InAction<int>((in int i) => { }));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithOutParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                // ReSharper disable once ObjectCreationAsStatement
                new EcsBuiltRunSystem(_filter, new OutAction<int>((out int i) => { i = 0; }));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithRefParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                // ReSharper disable once ObjectCreationAsStatement
                new EcsBuiltRunSystem(_filter, new RefAction<int>((ref int i) => { }));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithNonVoidReturnType_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                // ReSharper disable once ObjectCreationAsStatement
                new EcsBuiltRunSystem(_filter, new Func<int>(() => 0));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        private delegate void InAction<T>(in T a);

        private delegate void OutAction<T>(out T a);

        private delegate void RefAction<T>(ref T a);
    }
}