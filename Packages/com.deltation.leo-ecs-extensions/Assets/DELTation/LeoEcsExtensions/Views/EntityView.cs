using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Services;
using UnityEngine;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using Leopotam.Ecs;
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views
{
    [DisallowMultipleComponent]
    public class EntityView : MonoBehaviour, IEntityView
    {
        [SerializeField] private bool _createOnAwake = true;

        private readonly List<IEntityInitializer> _initializers = new List<IEntityInitializer>();
        private IActiveEcsWorld _activeWorld;

        private EcsPackedEntity _entity;

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

        public EcsPackedEntity GetOrCreateEntity()
        {
            if (TryGetEntity(out var entity))
                return entity;

            CreateEntity();
            return _entity;
        }

        public bool TryGetEntity(out EcsPackedEntity entity)
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

        protected virtual void AddComponents(EcsPackedEntity entity) { }

        protected virtual void OnCreatedEntity(EcsPackedEntity entity) { }

        protected virtual void OnAwake() { }

        protected virtual void OnDestroyed() { }
    }
}