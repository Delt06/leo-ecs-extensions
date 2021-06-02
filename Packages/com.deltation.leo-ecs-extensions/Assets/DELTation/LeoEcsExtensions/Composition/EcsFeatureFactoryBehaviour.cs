using System;
using JetBrains.Annotations;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Composition
{
    public abstract class EcsFeatureFactoryBehaviour : MonoBehaviour, IEcsFeatureFactory
    {
        public void AddFeatures(EcsSystems systems, EcsSystems physicsSystems)
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));
            if (physicsSystems == null) throw new ArgumentNullException(nameof(physicsSystems));

            var builder = new FeatureFactoryBuilder();
            ConfigureFeatures(builder);

            foreach (var feature in builder.Build())
            {
                feature.Register(systems, physicsSystems);
            }
        }

        protected abstract void ConfigureFeatures([NotNull] FeatureFactoryBuilder builder);
    }
}