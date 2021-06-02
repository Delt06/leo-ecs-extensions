using System;
using DELTation.LeoEcsExtensions.Features;
using JetBrains.Annotations;
using Leopotam.Ecs;
using static DELTation.DIFramework.Di;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class EcsSystemsExtensions
    {
        public static EcsSystems CreateAndAdd<TSystem>([NotNull] this EcsSystems systems)
            where TSystem : class, IEcsSystem
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));

            var system = Create<TSystem>();
            systems.Add(system);

            return systems;
        }

        public static (EcsSystems systems, EcsSystems physicsSystems) CreateAndAddFeature<T>(
            this (EcsSystems systems, EcsSystems physicsSystems) systems) where T : Feature
        {
            var feature = Create<T>();
            feature.Register(systems.systems, systems.physicsSystems);
            return systems;
        }
    }
}