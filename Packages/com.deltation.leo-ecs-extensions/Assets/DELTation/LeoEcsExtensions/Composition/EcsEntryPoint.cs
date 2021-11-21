#if LEOECS_EXTENSIONS_LITE
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

#else
using UnityEngine;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.Ecs;
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif
#endif

namespace DELTation.LeoEcsExtensions.Composition
{
    public abstract class EcsEntryPoint : MonoBehaviour, IActiveEcsWorld
    {
        private bool _initialized;
        private EcsSystems _lateSystems;
        private EcsSystems _physicsSystems;
        private EcsSystems _systems;
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
        private EcsSystems _editorSystems;
#endif

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
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems.Run();
#endif
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
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems?.Destroy();
#endif
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

#if UNITY_EDITOR && !LEOECS_EXTENSIONS_LITE
            EcsWorldObserver.Create(_world);
#endif

            _systems = new EcsSystems(_world, "Systems (Update)");
            _physicsSystems = new EcsSystems(_world, "Physics Systems (Fixed Update)");
            _lateSystems = new EcsSystems(_world, "Late Systems (Late Update)");

#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems = new EcsSystems(_world, "Editor Systems")
                    .Add(new EcsWorldDebugSystem())
                ;
            _editorSystems.Init();
#endif

            PopulateSystems(_systems, _world);
            PopulatePhysicsSystems(_physicsSystems, _world);
            PopulateLateSystems(_lateSystems, _world);


#if !LEOECS_EXTENSIONS_LITE
            Inject(_systems, _physicsSystems, _lateSystems);
            _systems.ProcessInjects();
            _physicsSystems.ProcessInjects();
            _lateSystems.ProcessInjects();
#endif

#if UNITY_EDITOR && !LEOECS_EXTENSIONS_LITE
            EcsSystemsObserver.Create(_systems);
            EcsSystemsObserver.Create(_physicsSystems);
            EcsSystemsObserver.Create(_lateSystems);
#endif
        }

        protected abstract void PopulateSystems([NotNull] EcsSystems systems, [NotNull] EcsWorld world);

        protected virtual void PopulatePhysicsSystems([NotNull] EcsSystems physicsSystems, [NotNull] EcsWorld world) { }

        protected virtual void PopulateLateSystems([NotNull] EcsSystems lateSystems, [NotNull] EcsWorld world) { }

#if !LEOECS_EXTENSIONS_LITE
        private void Inject(EcsSystems systems, EcsSystems physicsSystems, EcsSystems lateSystems)
        {
            var providers = GetComponentsInChildren<IEcsInjectionProvider>();

            foreach (var provider in providers)
            {
                provider.Inject(systems, physicsSystems, lateSystems);
            }
        }

#endif
    }
}