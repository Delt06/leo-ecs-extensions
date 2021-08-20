using System;
using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Features;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Composition
{
	public class FeatureFactoryBuilder
	{
		public FeatureFactoryBuilder Add([NotNull] Feature feature)
		{
			if (feature == null) throw new ArgumentNullException(nameof(feature));
			_features.Add(feature);
			return this;
		}

		public FeatureFactoryBuilder OneFrame<T>() where T : struct
		{
			_features.Add(new OneFrameFeature<T>());
			return this;
		}

		public IReadOnlyList<Feature> Build() => _features;

		private readonly List<Feature> _features = new List<Feature>();
	}
}