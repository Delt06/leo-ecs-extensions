﻿using DELTation.LeoEcsExtensions.Composition.Di;
using Runner.Collection;
using Runner.Movement;

namespace Runner
{
    public class RunnerEcsEntryPoint : EcsEntryPoint
    {
        public override void PopulateSystems(EcsFeatureBuilder featureBuilder)
        {
            featureBuilder
                .CreateAndAdd<SplineMovementSystem>()
                .CreateAndAdd<SidePositionSmoothSystem>()
                .CreateAndAdd<SplineSideMovementSystem>()
                ;

            featureBuilder
                .CreateAndAdd<CoinCollectionSystem>()
                .OneFrame<CollectCoinCommand>()
                ;
        }
    }
}