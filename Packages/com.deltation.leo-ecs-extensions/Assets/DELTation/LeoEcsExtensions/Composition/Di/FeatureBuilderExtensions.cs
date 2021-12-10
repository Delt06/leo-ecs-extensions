using System;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class FeatureBuilderExtensions
    {
        public static EcsFeatureBuilder CreateAndAdd<[MeansImplicitUse] TSystem>([NotNull] this EcsFeatureBuilder featureBuilder,
            [CanBeNull] string name = null) where TSystem : class, IEcsSystem
        {
            if (featureBuilder == null) throw new ArgumentNullException(nameof(featureBuilder));

            var system = DIFramework.Di.Create<TSystem>();
            return featureBuilder.Add(system, name);
        }
    }
}