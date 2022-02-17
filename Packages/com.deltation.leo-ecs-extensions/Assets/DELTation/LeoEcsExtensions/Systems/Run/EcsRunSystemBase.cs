using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public abstract class EcsRunSystemBase : IEcsRunSystem, IEcsPreInitSystem
    {
        private EcsBuiltRunSystem _builtRunSystem;

        public void PreInit(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _builtRunSystem = Build(world);
            OnAfterPreInit(systems);
        }

        public void Run(EcsSystems systems)
        {
            _builtRunSystem.Run();
        }

        protected virtual void OnAfterPreInit(EcsSystems systems) { }

        protected abstract EcsBuiltRunSystem Build(EcsWorld world);
    }
}