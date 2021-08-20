using System;
using DELTation.LeoEcsExtensions.Features;
using JetBrains.Annotations;
using static DELTation.DIFramework.Di;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
	public static class FeatureFactoryBuilderExtensions
	{
		public static FeatureFactoryBuilder CreateAndAdd<TFeature>([NotNull] this FeatureFactoryBuilder builder)
			where TFeature : Feature
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			var system = Create<TFeature>();
			builder.Add(system);

			return builder;
		}
	}
}