using System.Reflection;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace DELTation.LeoEcsExtensions.Tests.EditMode.Systems.Run
{
    public class EcsCallbackTests
    {
        private EcsFilter _queriedFilter;
        private EcsObservablePool<int> _queriedObservablePool;
        private EcsPool<int> _queriedPool;
        private EcsReadOnlyPool<int> _queriedReadOnlyPool;
        private EcsReadWritePool<int> _queriedReadWritePool;
        private EcsWorld _queriedWorld;
        private EcsWorld _world;

        [SetUp]
        public void SetUp()
        {
            _world = new EcsWorld();
            _queriedFilter = null;
            _queriedWorld = null;
            _queriedReadOnlyPool = default;
            _queriedReadWritePool = default;
            _queriedObservablePool = default;
        }

        private void QueryNothing() { }

        private void QueryFilter(EcsFilter filter, EcsPool<int> ints)
        {
            _queriedFilter = filter;
            _queriedPool = ints;
        }

        private void QueryFilterReadOnly(EcsFilter filter, EcsReadOnlyPool<int> ints)
        {
            _queriedFilter = filter;
            _queriedReadOnlyPool = ints;
        }

        private void QueryFilterReadWrite(EcsFilter filter, EcsReadWritePool<int> ints)
        {
            _queriedFilter = filter;
            _queriedReadWritePool = ints;
        }

        private void QueryFilterObservable(EcsFilter filter, EcsObservablePool<int> ints)
        {
            _queriedFilter = filter;
            _queriedObservablePool = ints;
        }

        private void QueryFilterCombined(EcsFilter filter, EcsWorld world, EcsObservablePool<int> ints)
        {
            _queriedFilter = filter;
            _queriedWorld = world;
            _queriedObservablePool = ints;
        }

        private void QueryFilterNoIncludes(EcsFilter filter)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterNoIncludesIgnore(EcsFilter filter, [EcsIgnore] EcsPool<int> ints)
        {
            _queriedFilter = filter;
            _queriedPool = ints;
        }

        private void QueryFilterNoIncludesExclude(EcsFilter filter, [EcsExc] EcsPool<int> ints)
        {
            _queriedFilter = filter;
            _queriedPool = ints;
        }

        private void QueryFilterComplex(EcsFilter filter, EcsPool<int> ints, EcsObservablePool<float> floats,
            [EcsIgnore] EcsReadOnlyPool<short> shorts, [EcsExc] EcsReadWritePool<byte> bytes)
        {
            _queriedFilter = filter;
            _queriedPool = ints;
        }

        private void QueryInvalid(int i) { }

        private void QueryInvalidIn(in int i) { }
        private string QueryInvalidNonVoid() => null;

        private void QueryInvalidOut(out int i)
        {
            i = 0;
        }

        // ReSharper disable once UnusedParameter.Local
        private void QueryInvalidRef(ref int i) { }


        private void QueryWorld(EcsWorld world)
        {
            _queriedWorld = world;
        }

        [Test]
        public void GivenRun_WhenQueryingNothing_ThenRunsWithoutExceptions()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryNothing));

            // Act
            void Code() => builtRunSystem.Run();

            // Assert
            Assert.That(Code, Throws.Nothing);
        }

        private MethodInfo GetMethod(string name)
        {
            var type = GetType();
            return type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private EcsCallback CreateCallback(string methodName) => new EcsCallback(_world, GetMethod(methodName), this);

        [Test]
        public void GivenRun_WhenQueryingEcsFilter_ThenReturnsTheProvidedFilter()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilter));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.EqualTo(_world.Filter<int>().End()));
        }

        [Test]
        public void GivenRun_WhenQueryingComplexEcsFilter_ThenReturnsFilterWithIncludesAndExcludes()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterComplex));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.EqualTo(_world.Filter<int>().Inc<float>().Exc<byte>().End()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsWorld_ThenReturnsTheUsedWorld()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryWorld));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedWorld, Is.Not.Null);
            Assume.That(_queriedWorld, Is.EqualTo(_world));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsPool_ThenReturnsCorrectPool()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilter));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedPool, Is.Not.Null);
            Assume.That(_queriedPool, Is.EqualTo(_world.GetPool<int>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsReadOnlyPool_ThenReturnsCorrectPool()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterReadOnly));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedReadOnlyPool, Is.Not.EqualTo(default(EcsReadOnlyPool<int>)));
            Assume.That(_queriedReadOnlyPool, Is.EqualTo(_world.GetReadOnlyPool<int>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsReadWritePool_ThenReturnsCorrectPool()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterReadWrite));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedReadWritePool, Is.Not.EqualTo(default(EcsReadWritePool<int>)));
            Assume.That(_queriedReadWritePool, Is.EqualTo(_world.GetReadWritePool<int>()));
        }

        [Test]
        public void GivenRun_WhenQueryingEcsObservablePool_ThenReturnsCorrectPool()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterObservable));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedObservablePool, Is.Not.EqualTo(default(EcsObservablePool<int>)));
            Assume.That(_queriedObservablePool, Is.EqualTo(_world.GetObservablePool<int>()));
        }

        [Test]
        public void GivenRun_WhenQueryingMultipleObjects_ThenReturnsAll()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterCombined));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedObservablePool, Is.Not.EqualTo(default(EcsObservablePool<int>)));
            Assume.That(_queriedObservablePool, Is.EqualTo(_world.GetObservablePool<int>()));

            Assert.That(_queriedWorld, Is.Not.Null);
            Assume.That(_queriedWorld, Is.EqualTo(_world));

            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.EqualTo(_world.Filter<int>().End()));
        }

        [Test]
        public void GivenRun_WhenQueryingUnsupportedType_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code()
            {
                CreateCallback(nameof(QueryInvalid));
            }

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithInParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryInvalidIn));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithOutParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryInvalidOut));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithRefParameter_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryInvalidRef));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingWithNonVoidReturnType_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryInvalidNonVoid));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingFilterWithoutIncludes_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryFilterNoIncludes));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingFilterWithoutIncludesAndIgnoredPool_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryFilterNoIncludesIgnore));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }

        [Test]
        public void GivenRun_WhenQueryingFilterWithoutIncludesAndExcludedPool_ThenThrowsInvalidRunSignatureException()
        {
            // Act
            void Code() => CreateCallback(nameof(QueryFilterNoIncludesExclude));

            // Assert
            Assert.That(Code, Throws.InstanceOf<InvalidRunSignatureException>());
        }
    }
}