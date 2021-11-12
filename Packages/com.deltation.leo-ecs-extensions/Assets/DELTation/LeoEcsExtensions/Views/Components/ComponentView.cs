using Leopotam.Ecs;
using UnityEngine;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class ComponentView<T> : MonoBehaviour, IEntityInitializer where T : struct
    {
        [SerializeField]
#if UNITY_EDITOR && ODIN_INSPECTOR
        [HideIf(nameof(EntityIsAlive))]
#endif
        private bool _enabled = true;

        [SerializeField]
#if UNITY_EDITOR && ODIN_INSPECTOR
        [HideIf(nameof(ComponentExists))] [InlineProperty] [HideLabel]
#endif
        private T _component;

        private EcsEntity _entity = EcsEntity.Null;

        public ref T StoreComponentValue => ref _component;

        public void InitializeEntity(EcsEntity entity)
        {
            if (_enabled)
            {
                PreInitializeEntity(entity);
                entity.Replace(_component);
                PostInitializeEntity(entity);
            }

#if UNITY_EDITOR && ODIN_INSPECTOR
            _entity = entity;
#endif
        }

        protected virtual void PreInitializeEntity(EcsEntity entity) { }
        protected virtual void PostInitializeEntity(EcsEntity entity) { }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [ShowInInspector] [ShowIf(nameof(ComponentExists))] [InlineProperty] [HideLabel]
        private T AttachedComponentView
        {
            get => ComponentExists ? _entity.Get<T>() : default;
            set
            {
                if (EntityIsAlive)
                    _entity.Replace(value);
            }
        }

        [Button] [ShowIf(nameof(ComponentExists))] [GUIColor(0.75f, 0.25f, 0.25f)]
        private void DeleteComponent()
        {
            if (EntityIsAlive)
                _entity.Del<T>();
        }

        [Button] [ShowIf(nameof(ShowAddComponent))] [GUIColor(0.25f, 0.75f, 0.25f)]
        private void AddComponent()
        {
            if (EntityIsAlive)
                _entity.Replace(_component);
        }

        private bool ShowAddComponent => EntityIsAlive && !_entity.Has<T>();

        private bool ComponentExists => EntityIsAlive && _entity.Has<T>();

        private bool EntityIsAlive => _entity.IsAlive();
#endif
    }
}