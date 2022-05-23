using System;
using JetBrains.Annotations;
using Leopotam.EcsLite;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public abstract class EcsEntryPoint
    {
        private bool _initialized;
        private EcsSystems _lateSystems;
        private EcsSystems _physicsSystems;
        private EcsSystems _systems;


        protected EcsEntryPoint() => World = new EcsWorld();

        public EcsWorld World { get; }

        public void Start()
        {
            _systems.Init();
            _physicsSystems.Init();
            _lateSystems.Init();
        }

        public void Update()
        {
#if UNITY_EDITOR
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
#if UNITY_EDITOR
            _editorSystems?.Destroy();
#endif
            _systems?.Destroy();
            _physicsSystems?.Destroy();
            World?.Destroy();
        }

        internal void Initialize(Action<EcsSystems> populateSystems, Action<EcsSystems> populatePhysicsSystems,
            Action<EcsSystems> populateLateSystems)
        {
            if (_initialized) throw new InvalidOperationException("Already initialized.");

            _initialized = true;

            _systems = new EcsSystems(World, "Systems (Update)");
            _physicsSystems = new EcsSystems(World, "Physics Systems (Fixed Update)");
            _lateSystems = new EcsSystems(World, "Late Systems (Late Update)");

#if UNITY_EDITOR
            DebugSystem = new EcsWorldDebugSystem();
            _editorSystems = new EcsSystems(World, "Editor Systems")
                    .Add(DebugSystem)
                ;
            _editorSystems.Init();
#endif

            populateSystems(_systems);
            populatePhysicsSystems(_physicsSystems);
            populateLateSystems(_lateSystems);
        }

        public abstract void PopulateSystems([NotNull] EcsFeatureBuilder featureBuilder);

        public virtual void PopulatePhysicsSystems([NotNull] EcsFeatureBuilder featureBuilder) { }

        public virtual void PopulateLateSystems([NotNull] EcsFeatureBuilder featureBuilder) { }
#if UNITY_EDITOR
        private EcsSystems _editorSystems;
        [CanBeNull]
        public EcsWorldDebugSystem DebugSystem { get; private set; }
#endif

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
    }
}