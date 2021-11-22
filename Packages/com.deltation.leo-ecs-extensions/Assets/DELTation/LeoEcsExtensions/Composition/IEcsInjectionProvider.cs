#if !LEOECS_EXTENSIONS_LITE
using JetBrains.Annotations;
using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Composition
{
    public interface IEcsInjectionProvider
    {
        void Inject([NotNull] EcsSystems systems, [NotNull] EcsSystems physicsSystems,
            [NotNull] EcsSystems lateSystems);
    }
}

#endif