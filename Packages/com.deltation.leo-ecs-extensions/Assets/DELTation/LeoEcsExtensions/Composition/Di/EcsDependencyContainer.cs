#if LEOECS_EXTENSIONS_LITE
using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using DELTation.DIFramework.Dependencies;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class ContainerBuilderExtensions
    {
        public static ICanRegisterContainerBuilder RegisterEcsEntryPoint(
            [NotNull] this ICanRegisterContainerBuilder containerBuilder, [NotNull] EcsEntryPoint ecsEntryPoint)
        {
            if (containerBuilder == null)
                throw new ArgumentNullException(nameof(containerBuilder));

            if (ecsEntryPoint == null) return containerBuilder;

            containerBuilder.Register(ecsEntryPoint.World);

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
            foreach (var systemAsDependency in featureBuilder.SystemsAsDependencies)
            {
                secondaryDependencies.Add(systemAsDependency);
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
                    systems.Add((IEcsSystem) secondaryObjects[i]);
                }
            };
    }
}

#endif