using System.Reflection;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using DELTation.LeoEcsExtensions.Systems.Run.Exceptions;
using DELTation.LeoEcsExtensions.Utilities;
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
        private EcsSingletonPool<float> _queriedSingletonPool;
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

        private void QueryFilterNoIncludesExclude([EcsExc(typeof(int))] EcsFilter filter)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterTwoExcludes([EcsExc(typeof(int))] [EcsExc(typeof(float))] EcsFilter filter,
            EcsPool<byte> pool)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterComplex([EcsExc(typeof(byte))] EcsFilter filter, EcsPool<int> ints,
            EcsObservablePool<float> floats,
            [EcsIgnore] EcsReadOnlyPool<short> shorts)
        {
            _queriedFilter = filter;
            _queriedPool = ints;
        }

        private void QueryFilterUpdateOnFirstPool(EcsFilter filter, [EcsIncUpdate] EcsPool<int> ints)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterIncludeNoPools([EcsInc(typeof(int))] EcsFilter filter)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterIncludeAndPools([EcsInc(typeof(int))] EcsFilter filter, EcsPool<float> floats)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterUpdateOnSecondPool(EcsFilter filter, EcsPool<float> floats,
            [EcsIncUpdate] EcsPool<int> ints)
        {
            _queriedFilter = filter;
        }

        private void QueryFilterAndSingleton(EcsFilter filter, EcsPool<int> ints, EcsSingletonPool<float> floats)
        {
            _queriedFilter = filter;
            _queriedSingletonPool = floats;
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
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().End()));
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
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().Inc<float>().Exc<byte>().End()));
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
            Assume.That(_queriedPool, Is.SameAs(_world.GetPool<int>()));
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
            Assume.That(_queriedWorld, Is.SameAs(_world));

            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingIncUpdateOnFirstPool_ThenFilterIsConstructedCorrectly()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterUpdateOnFirstPool));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.FilterAndIncUpdateOf<int>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingIncUpdateOnSecondPool_ThenFilterIsConstructedCorrectly()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterUpdateOnSecondPool));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<float>().IncComponentAndUpdateOf<int>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingTwoExcAttributes_ThenFilterExcludesBoth()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterTwoExcludes));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<byte>().Exc<float>().Exc<int>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingOnlyIncAttribute_ThenFilterIncludesOnlyTypeFromInc()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterIncludeNoPools));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingIncAttributeAndPool_ThenFilterIncludesBoth()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterIncludeAndPools));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().Inc<float>().End()));
        }

        [Test]
        public void GivenRun_WhenHavingFilterAndSingletonPool_ThenSingletonPoolIsIgnored()
        {
            // Arrange
            var builtRunSystem = CreateCallback(nameof(QueryFilterAndSingleton));

            // Act
            builtRunSystem.Run();

            // Assert
            Assert.That(_queriedFilter, Is.Not.Null);
            Assume.That(_queriedFilter, Is.SameAs(_world.Filter<int>().End()));

            Assert.That(_queriedSingletonPool, Is.Not.EqualTo(default(EcsSingletonPool<float>)));
            Assert.That(_queriedSingletonPool, Is.EqualTo(_world.GetSingletonPool<float>()));
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