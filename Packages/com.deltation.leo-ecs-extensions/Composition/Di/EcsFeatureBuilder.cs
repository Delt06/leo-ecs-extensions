using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using DELTation.DIFramework.Dependencies;
using DELTation.LeoEcsExtensions.Systems;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public sealed class EcsFeatureBuilder
    {
        // ReSharper disable once NotAccessedField.Local
        [CanBeNull] private readonly string _name;
        internal readonly List<IDependency> SystemsAsDependencies =
            new List<IDependency>();

        internal EcsFeatureBuilder([CanBeNull] string name = null) => _name = name;

        public EcsFeatureBuilder Add([NotNull] IEcsSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            SystemsAsDependencies.Add(new ObjectDependency(system));
            return this;
        }

        public EcsFeatureBuilder OneFrame<T>() where T : struct
        {
            SystemsAsDependencies.Add(new ObjectDependency(new RemoveOneFrame<T>()));
            return this;
        }

        public EcsFeatureBuilder OneFrameEntity<T>() where T : struct
        {
            SystemsAsDependencies.Add(new ObjectDependency(new RemoveOneFrameEntity<T>()));
            return this;
        }

        public EcsFeatureBuilder CreateAndAdd<[MeansImplicitUse] TSystem>()
            where TSystem : class, IEcsSystem
        {
            SystemsAsDependencies.Add(new TypeDependency(typeof(TSystem)));
            return this;
        }

        public EcsFeatureBuilder CreateViaFactoryMethodAndAdd<TR>([NotNull] FactoryMethod<TR> factoryMethod)
            where TR : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }

        public EcsFeatureBuilder CreateViaFactoryMethodAndAdd<TR, T1>([NotNull] FactoryMethod<TR, T1> factoryMethod)
            where TR : class where T1 : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }

        public EcsFeatureBuilder CreateViaFactoryMethodAndAdd<TR, T1, T2>(
            [NotNull] FactoryMethod<TR, T1, T2> factoryMethod) where TR : class where T1 : class where T2 : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }

        public EcsFeatureBuilder CreateViaFactoryMethodAndAdd<TR, T1, T2, T3>(
            [NotNull] FactoryMethod<TR, T1, T2, T3> factoryMethod) where TR : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }

        public EcsFeatureBuilder
            CreateViaFactoryMethodAndAdd<TR, T1, T2, T3, T4>([NotNull] FactoryMethod<TR, T1, T2, T3, T4> factoryMethod)
            where TR : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }

        public EcsFeatureBuilder
            CreateViaFactoryMethodAndAdd<TR, T1, T2, T3, T4, T5>(
                [NotNull] FactoryMethod<TR, T1, T2, T3, T4, T5> factoryMethod) where TR : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));
            SystemsAsDependencies.Add(new FactoryMethodDelegateDependency(factoryMethod));
            return this;
        }
    }
}