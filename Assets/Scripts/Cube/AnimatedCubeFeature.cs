using DELTation.LeoEcsExtensions.Composition;

namespace Cube
{
	public class AnimatedCubeFeature : PrebuiltFeature
	{
		protected override void ConfigureBuilder(EcsFeatureBuilder featureBuilder)
		{
			featureBuilder
				.Add(new CubeSpawnSystem())
				.Add(new CubeTranslationSystem())
				.Add(new CubeRotationSystem())
				;
		}
	}
}