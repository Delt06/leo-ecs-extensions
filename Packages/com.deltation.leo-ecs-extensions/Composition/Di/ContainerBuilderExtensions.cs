using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using DELTation.DIFramework.Dependencies;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Scripting;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class ContainerBuilderExtensions
    {
        public static ICanRegisterContainerBuilder AttachEcsEntryPointViewTo(
            [NotNull] this ICanRegisterContainerBuilder containerBuilder, [NotNull] GameObject gameObject)
        {
            if (containerBuilder == null) throw new ArgumentNullException(nameof(containerBuilder));

            if (gameObject != null)
                return containerBuilder.RegisterFromMethod((EcsEntryPoint ecsEntryPoint) =>
                    {
                        var ecsEntryPointView = gameObject.AddComponent<EcsEntryPointView>();
                        ecsEntryPointView.Construct(ecsEntryPoint);
                        return ecsEntryPointView;
                    }
                ).AsInternal();

            if (Application.isPlaying)
                throw new ArgumentNullException(nameof(gameObject));

            return containerBuilder;
        }

        public static ICanRegisterContainerBuilder RegisterEcsEntryPoint<TEcsEntryPoint>(
            [NotNull] this ICanRegisterContainerBuilder containerBuilder) where TEcsEntryPoint : EcsEntryPoint, new()
        {
            if (containerBuilder == null)
                throw new ArgumentNullException(nameof(containerBuilder));


            var ecsEntryPoint = new TEcsEntryPoint();
            containerBuilder.Register(ecsEntryPoint.World);
            containerBuilder.RegisterFromMethod(() => new MainEcsWorld(ecsEntryPoint.World));

            var secondaryDependencies = new List<IDependency>();

            PopulateSystems(secondaryDependencies, ecsEntryPoint.PopulateSystems,
                out var systemsStartIndex, out var systemsEndIndexExclusive
            );
            PopulateSystems(secondaryDependencies, ecsEntryPoint.PopulatePhysicsSystems,
                out var physicsSystemsStartIndex, out var physicsSystemsEndIndexExclusive
            );
            PopulateSystems(secondaryDependencies, ecsEntryPoint.PopulateLateSystems,
                out var lateSystemsStartIndex, out var lateSystemsEndIndexExclusive
            );

            // this one always gets resolved
            // it is added only to display a dependency on EcsWorld
            secondaryDependencies.Add(new TypeDependency(typeof(DummyEcsWorldDependency)));

            var compositeDependency = new CompositeDependency(new ObjectDependency(ecsEntryPoint),
                secondaryDependencies,
                Combine(systemsStartIndex, systemsEndIndexExclusive, physicsSystemsStartIndex,
                    physicsSystemsEndIndexExclusive, lateSystemsStartIndex, lateSystemsEndIndexExclusive
                )
            );
            containerBuilder.RegisterCompositeDependency(compositeDependency);

            return containerBuilder;
        }

        private static void PopulateSystems(ICollection<IDependency> secondaryDependencies,
            Action<EcsFeatureBuilder> populate, out int startIndex,
            out int endIndexExclusive)
        {
            startIndex = secondaryDependencies.Count;
            var featureBuilder = new EcsFeatureBuilder();
            populate(featureBuilder);
            AddToSecondaryDependencies(featureBuilder, secondaryDependencies);
            endIndexExclusive = secondaryDependencies.Count;
        }

        private static void AddToSecondaryDependencies(EcsFeatureBuilder featureBuilder,
            ICollection<IDependency> secondaryDependencies)
        {
            foreach (var dependency in featureBuilder.Dependencies)
            {
                secondaryDependencies.Add(dependency);
            }
        }

        private static CompositeDependency.CombinationHandler Combine(int systemsStartIndex,
            int systemsEndIndexExclusive, int physicsSystemsStartIndex, int physicsSystemsEndIndexExclusive,
            int lateSystemsStartIndex, int lateSystemsEndIndexExclusive)
        {
            return (primaryObject, secondaryObjects) =>
            {
                var primaryObjectAsEntryPoint = (EcsEntryPoint) primaryObject;
                primaryObjectAsEntryPoint.Initialize(
                    PopulateSystemsRange(secondaryObjects, systemsStartIndex, systemsEndIndexExclusive),
                    PopulateSystemsRange(secondaryObjects, physicsSystemsStartIndex, physicsSystemsEndIndexExclusive
                    ),
                    PopulateSystemsRange(secondaryObjects, lateSystemsStartIndex, lateSystemsEndIndexExclusive)
                );
            };
        }

        private static Action<EcsSystems> PopulateSystemsRange(IReadOnlyList<object> secondaryObjects, int startIndex,
            int endIndexExclusive) =>
            systems =>
            {
                for (var i = startIndex; i < endIndexExclusive; i++)
                {
                    var secondaryObject = secondaryObjects[i];
                    switch (secondaryObject)
                    {
                        case IEcsSystem system:
                            systems.Add(system);
                            break;
                        case ISystemBuilder systemBuilder:
                            systemBuilder.Populate(systems);
                            break;
                    }
                }
            };

        [Preserve]
        private class DummyEcsWorldDependency
        {
            [Preserve]
            public DummyEcsWorldDependency(EcsWorld world) { }
        }
    }
}