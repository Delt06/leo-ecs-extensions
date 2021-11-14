using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Services;
using DELTation.LeoEcsExtensions.Views.Blueprints;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views
{
    [DisallowMultipleComponent]
    public class EntityView : MonoBehaviour, IEntityView
    {
        [SerializeField] private bool _createOnAwake = true;
        [Tooltip("Optional blueprint for created entity.")] [SerializeField]
        private EntityBlueprint _blueprint;

        private readonly List<IEntityInitializer> _initializers = new List<IEntityInitializer>();
        private IActiveEcsWorld _activeWorld;
        private EcsEntity _entity = EcsEntity.Null;
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

        public EcsEntity Entity
        {
            get
            {
                if (TryGetEntity(out var entity))
                    return entity;

                CreateEntity();
                return _entity;
            }
        }

        public bool TryGetEntity(out EcsEntity entity)
        {
            if (!_entity.IsNull() && _entity.IsAlive())
            {
                entity = _entity;
                return true;
            }

            entity = EcsEntity.Null;
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
            _entity = World.NewEntity();
            _entity.SetUnityObjectData(transform);
            _entity.SetViewBackRef(this);

            if (_blueprint)
                _blueprint.InitializeEntity(_entity);

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
            if (!IsValid(_entity)) return;
            _entity.Destroy();
            _entity = EcsEntity.Null;
        }

        public EcsWorld World => _activeWorld.World;

        protected virtual void AddComponents(EcsEntity entity) { }

        protected virtual void OnCreatedEntity(EcsEntity entity) { }

        private static bool IsValid(in EcsEntity entity) => !entity.IsNull() && entity.IsAlive();

        protected virtual void OnAwake() { }

        protected virtual void OnDestroyed() { }
    }
}