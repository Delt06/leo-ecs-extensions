using System;
using System.Linq;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Composition
{
    public abstract class PrebuiltFeature
    {
        private string Name => GetFriendlyNameOf(GetType());

        public void AddTo([NotNull] EcsSystems parentSystems)
        {
            if (parentSystems == null) throw new ArgumentNullException(nameof(parentSystems));
            var featureBuilder = parentSystems.StartBuildingFeature(Name);
            ConfigureBuilder(featureBuilder);
            featureBuilder.Build();
        }

        private static string GetFriendlyNameOf(Type type)
        {
            // https://stackoverflow.com/questions/4185521/c-sharp-get-generic-type-name
            // Answer by Ali
            var friendlyName = type.Name;
            if (!type.IsGenericType) return friendlyName;

            var iBacktick = friendlyName.IndexOf('`');
            if (iBacktick > 0) friendlyName = friendlyName.Remove(iBacktick);

            var genericParameters = type.GetGenericArguments().Select(GetFriendlyNameOf);
            friendlyName += "<" + string.Join(", ", genericParameters) + ">";

            return friendlyName;
        }

        protected abstract void ConfigureBuilder([NotNull] EcsFeatureBuilder featureBuilder);
    }
}