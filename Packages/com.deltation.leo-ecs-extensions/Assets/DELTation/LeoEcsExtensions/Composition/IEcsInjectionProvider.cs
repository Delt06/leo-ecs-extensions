using JetBrains.Annotations;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Composition
{
    public interface IEcsInjectionProvider
    {
        public void Inject([NotNull] EcsSystems systems, [NotNull] EcsSystems physicsSystems);
    }
}