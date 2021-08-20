using DELTation.LeoEcsExtensions.Composition;
using DELTation.LeoEcsExtensions.Systems;

namespace Cube
{
	public class AnimatedCubeFeature : PrebuiltFeature
	{
		protected override void ConfigureBuilder(EcsFeatureBuilder featureBuilder)
		{
			featureBuilder
				.Add(new CubeSpawnSystem())
				.Add(new ReadFromTransformsSystem())
				.Add(new CubeTranslationSystem())
				.Add(new CubeRotationSystem())
				.Add(new WriteToTransformsSystem())
				;
		}
	}
}