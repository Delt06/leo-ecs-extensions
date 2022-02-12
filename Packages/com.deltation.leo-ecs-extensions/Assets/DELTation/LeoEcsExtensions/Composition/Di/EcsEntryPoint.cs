#if LEOECS_EXTENSIONS_LITE
using System;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
#if UNITY_EDITOR
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

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public abstract class EcsEntryPoint : IActiveEcsWorld
    {
        private bool _initialized;
        private EcsSystems _lateSystems;
        private EcsSystems _physicsSystems;
        private EcsSystems _systems;
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
        private EcsSystems _editorSystems;
#endif

        protected EcsEntryPoint() => World = new EcsWorld();

#if UNITY_EDITOR
        [CanBeNull]
        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        internal EcsSystems LateSystems => _lateSystems;

        [CanBeNull]
        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        internal EcsSystems PhysicsSystems => _physicsSystems;

        [CanBeNull]
        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        internal EcsSystems Systems => _systems;
#endif

        public void Start()
        {
            _systems.Init();
            _physicsSystems.Init();
            _lateSystems.Init();
        }

        public void Update()
        {
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems.Run();
#endif
            _systems.Run();
        }

        public void FixedUpdate()
        {
            _physicsSystems.Run();
        }

        public void LateUpdate()
        {
            _lateSystems.Run();
        }

        public void OnDestroy()
        {
#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems?.Destroy();
#endif
            _systems?.Destroy();
            _physicsSystems?.Destroy();
            World?.Destroy();
        }

        public EcsWorld World { get; }

        internal void Initialize(Action<EcsSystems> populateSystems, Action<EcsSystems> populatePhysicsSystems,
            Action<EcsSystems> populateLateSystems)
        {
            if (_initialized) throw new InvalidOperationException("Already initialized.");

            _initialized = true;

#if UNITY_EDITOR && !LEOECS_EXTENSIONS_LITE
            EcsWorldObserver.Create(_world);
#endif

            _systems = new EcsSystems(World, "Systems (Update)");
            _physicsSystems = new EcsSystems(World, "Physics Systems (Fixed Update)");
            _lateSystems = new EcsSystems(World, "Late Systems (Late Update)");

#if UNITY_EDITOR && LEOECS_EXTENSIONS_LITE
            _editorSystems = new EcsSystems(World, "Editor Systems")
                    .Add(new EcsWorldDebugSystem())
                ;
            _editorSystems.Init();
#endif

            populateSystems(_systems);
            populatePhysicsSystems(_physicsSystems);
            populateLateSystems(_lateSystems);


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

        public abstract void PopulateSystems([NotNull] EcsFeatureBuilder featureBuilder);

        public virtual void PopulatePhysicsSystems([NotNull] EcsFeatureBuilder featureBuilder) { }

        public virtual void PopulateLateSystems([NotNull] EcsFeatureBuilder featureBuilder) { }

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