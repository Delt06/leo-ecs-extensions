using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.Ecs;
using UnityEngine;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif

namespace DELTation.LeoEcsExtensions.Composition
{
    public abstract class EcsEntryPoint : MonoBehaviour, IActiveEcsWorld
    {
        private bool _initialized;
        private EcsSystems _lateSystems;
        private EcsSystems _physicsSystems;
        private EcsSystems _systems;
        private EcsWorld _world;

        protected void Start()
        {
            EnsureInitialized();

            _systems.Init();
            _physicsSystems.Init();
            _lateSystems.Init();
        }

        protected void Update()
        {
            _systems.Run();
        }

        protected void FixedUpdate()
        {
            _physicsSystems.Run();
        }

        protected void LateUpdate()
        {
            _lateSystems.Run();
        }

        protected void OnDestroy()
        {
            _systems?.Destroy();
            _physicsSystems?.Destroy();
            _world?.Destroy();
        }

        public EcsWorld World
        {
            get
            {
                EnsureInitialized();
                return _world;
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            _initialized = true;
            _world = new EcsWorld();

#if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
#endif

            _systems = new EcsSystems(_world, "Systems (Update)");
            _physicsSystems = new EcsSystems(_world, "Physics Systems (Fixed Update)");
            _lateSystems = new EcsSystems(_world, "Late Systems (Late Update)");

            PopulateSystems(_systems, _world);
            PopulatePhysicsSystems(_physicsSystems, _world);
            PopulateLateSystems(_lateSystems, _world);
            Inject(_systems, _physicsSystems, _lateSystems);

            _systems.ProcessInjects();
            _physicsSystems.ProcessInjects();
            _lateSystems.ProcessInjects();

#if UNITY_EDITOR
            EcsSystemsObserver.Create(_systems);
            EcsSystemsObserver.Create(_physicsSystems);
            EcsSystemsObserver.Create(_lateSystems);
#endif
        }

        protected abstract void PopulateSystems([NotNull] EcsSystems systems, [NotNull] EcsWorld world);

        protected virtual void PopulatePhysicsSystems([NotNull] EcsSystems physicsSystems, [NotNull] EcsWorld world) { }

        protected virtual void PopulateLateSystems([NotNull] EcsSystems lateSystems, [NotNull] EcsWorld world) { }

        private void Inject(EcsSystems systems, EcsSystems physicsSystems, EcsSystems lateSystems)
        {
            var providers = GetComponentsInChildren<IEcsInjectionProvider>();

            foreach (var provider in providers)
            {
                provider.Inject(systems, physicsSystems, lateSystems);
            }
        }
    }
}