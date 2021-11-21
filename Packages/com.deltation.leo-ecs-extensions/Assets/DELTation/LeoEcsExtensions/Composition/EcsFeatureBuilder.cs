using System;
using System.Collections.Generic;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Composition
{
    public sealed class EcsFeatureBuilder
    {
        // ReSharper disable once NotAccessedField.Local
        [CanBeNull] private readonly string _name;
        private readonly EcsSystems _parentSystems;
        private readonly List<(IEcsSystem system, string name)> _systems = new List<(IEcsSystem system, string name)>();
        private bool _isBuilt;

        public EcsWorld World =>
#if LEOECS_EXTENSIONS_LITE
            _parentSystems.GetWorld();
#else
            _parentSystems.World;
#endif


        internal EcsFeatureBuilder([NotNull] EcsSystems parentSystems, [CanBeNull] string name)
        {
            _parentSystems = parentSystems ?? throw new ArgumentNullException(nameof(parentSystems));
            _name = name;
        }

        public EcsFeatureBuilder Add([NotNull] IEcsSystem system, [CanBeNull] string systemName = null)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            _systems.Add((system, systemName));
            return this;
        }

        public EcsFeatureBuilder OneFrame<T>() where T : struct
        {
            _systems.Add((new RemoveOneFrame<T>(), null));
            return this;
        }

        public void Build()
        {
            if (_isBuilt) throw new InvalidOperationException("Builder has already built the feature.");

            _isBuilt = true;
#if LEOECS_EXTENSIONS_LITE

            foreach (var (system, _) in _systems)
            {
                _parentSystems.Add(system);
            }

#else
            var systems = new EcsSystems(_parentSystems.World, _name);

            foreach (var (system, name) in _systems)
            {
                systems.Add(system, name);
            }

            _parentSystems.Add(systems);
#endif
        }
    }
}