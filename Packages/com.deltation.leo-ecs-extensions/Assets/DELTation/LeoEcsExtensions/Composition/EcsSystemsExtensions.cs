using System;
using DELTation.LeoEcsExtensions.Systems;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif


namespace DELTation.LeoEcsExtensions.Composition
{
    public static class EcsSystemsExtensions
    {
        public static EcsFeatureBuilder StartBuildingFeature([NotNull] this EcsSystems parentSystems,
            [CanBeNull] string name = null)
        {
            if (parentSystems == null) throw new ArgumentNullException(nameof(parentSystems));
            return new EcsFeatureBuilder(parentSystems, name);
        }

        public static EcsSystems AddFeature([NotNull] this EcsSystems parentSystems,
            [NotNull] PrebuiltFeature prebuiltFeature)
        {
            if (parentSystems == null) throw new ArgumentNullException(nameof(parentSystems));
            if (prebuiltFeature == null) throw new ArgumentNullException(nameof(prebuiltFeature));

            prebuiltFeature.AddTo(parentSystems);
            return parentSystems;
        }

        public static EcsSystems OneFrameUpdateEvents([NotNull] this EcsSystems systems)
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));

            systems.Add(new OneFrameUpdateEventsSystem());
            return systems;
        }

        public static EcsSystems OneFrame<T>([NotNull] this EcsSystems systems) where T : struct
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));

            systems.Add(new RemoveOneFrame<T>());
            return systems;
        }
    }
}