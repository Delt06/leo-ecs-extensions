using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views
{
    [DisallowMultipleComponent]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class EntityView : MonoBehaviour, IEntityView
    {
        [SerializeField] private bool _createOnAwake = true;

        private readonly List<IEntityInitializer> _initializers = new List<IEntityInitializer>();
        private IActiveEcsWorld _activeWorld;

        private EcsPackedEntityWithWorld _entity;

        private bool _searchedForInitializers;

        public void Construct(IActiveEcsWorld activeWorld)
        {
            _activeWorld = activeWorld;
        }

        private List<IEntityInitializer> Initializers
        {
            get
            {
                if (!_searchedForInitializers)
                {
                    EntityViewUtils.FindAllEntityInitializers(transform, _initializers);
                    _searchedForInitializers = true;
                }

                return _initializers;
            }
        }

        protected void Awake()
        {
            OnAwake();
            if (_createOnAwake)
                CreateEntity();
        }

        protected void OnDestroy()
        {
            DestroyEntity();
            OnDestroyed();
        }

        public EcsPackedEntityWithWorld GetOrCreateEntity()
        {
            if (TryGetEntity(out var entity))
                return entity;

            CreateEntity();
            return _entity;
        }

        public bool TryGetEntity(out EcsPackedEntityWithWorld entity)
        {
            if (_entity.IsAliveCompatible())
            {
                entity = _entity;
                return true;
            }

            entity = default;
            return false;
        }

        public virtual void Destroy()
        {
            DestroyEntity();
            Destroy(gameObject);
        }

        public void CreateEntity()
        {
            if (!World.IsAlive())
            {
                Debug.LogError("Can't create entity: world is destroyed.", this);
                return;
            }

            DestroyEntity();
            _entity = World.NewPackedEntityCompatible();
            _entity.SetUnityObjectData(transform);
            _entity.SetViewBackRef(this);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < Initializers.Count; index++)
            {
                var initializer = Initializers[index];
                initializer.InitializeEntity(_entity);
            }

            AddComponents(_entity);
            OnCreatedEntity(_entity);
        }

        public void DestroyEntity()
        {
            if (!_entity.IsAliveCompatible()) return;

            _entity.DestroyCompatible();
            _entity = default;
        }

        public EcsWorld World => _activeWorld.World;

        protected virtual void AddComponents(EcsPackedEntityWithWorld entity) { }

        protected virtual void OnCreatedEntity(EcsPackedEntityWithWorld entity) { }

        protected virtual void OnAwake() { }

        protected virtual void OnDestroyed() { }
    }
}