using System;
using DELTation.LeoEcsExtensions.Systems;
#if LEOECS_EXTENSIONS_LITE
using JetBrains.Annotations;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public static class EcsFeatureBuilderExtensions
    {
        public static EcsFeatureBuilder OneFrameUpdateEvents([NotNull] this EcsFeatureBuilder featureBuilder)
        {
            if (featureBuilder == null) throw new ArgumentNullException(nameof(featureBuilder));

            featureBuilder.Add(new OneFrameUpdateEventsSystem());
            return featureBuilder;
        }

        public static EcsFeatureBuilder AddFeature<TPrebuiltFeature>([NotNull] this EcsFeatureBuilder featureBuilder)
            where TPrebuiltFeature : PrebuiltFeature, new()
        {
            var feature = new TPrebuiltFeature();
            feature.Add(featureBuilder);
            return featureBuilder;
        }

        public static EcsFeatureBuilder AddFeature([NotNull] this EcsFeatureBuilder featureBuilder,
            [NotNull] PrebuiltFeature feature)
        {
            if (feature == null) throw new ArgumentNullException(nameof(feature));
            feature.Add(featureBuilder);
            return featureBuilder;
        }
    }
}