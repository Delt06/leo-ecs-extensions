#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
#else
using Leopotam.Ecs;
#endif
using System;
using JetBrains.Annotations;
using static DELTation.DIFramework.Di;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class EcsSystemsExtensions
    {
        public static EcsSystems CreateAndAdd<[MeansImplicitUse] TSystem>([NotNull] this EcsSystems systems)
            where TSystem : class, IEcsSystem
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));

            var system = Create<TSystem>();
            systems.Add(system);

            return systems;
        }

        public static EcsSystems CreateAndAddFeature<[MeansImplicitUse] TFeature>([NotNull] this EcsSystems systems)
            where TFeature : PrebuiltFeature
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));

            var feature = Create<TFeature>();
            return systems.AddFeature(feature);
        }
    }
}