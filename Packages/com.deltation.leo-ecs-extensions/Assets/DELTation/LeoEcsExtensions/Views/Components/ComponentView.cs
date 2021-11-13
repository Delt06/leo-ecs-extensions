using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class ComponentView : MonoBehaviour
    {
        public EcsEntity Entity { get; protected set; } = EcsEntity.Null;

        public abstract void TryAddComponent();
        public abstract void TryDeleteComponent();
        public abstract bool EntityHasComponent();
    }

    public abstract class ComponentView<T> : ComponentView, IEntityInitializer where T : struct
    {
        [SerializeField] private bool _enabled = true;

        [SerializeField] private T _component;

        public ref T StoredComponentValue => ref _component;


        protected virtual void OnValidate()
        {
            if (!Entity.IsAlive()) return;
            if (!Entity.Has<T>()) return;

            var attachedComponent = Entity.Get<T>();
            if (!Equals(attachedComponent, _component))
                Entity.Replace(_component);
        }

        public void InitializeEntity(EcsEntity entity)
        {
            if (_enabled)
            {
                PreInitializeEntity(entity);
                entity.Replace(_component);
                PostInitializeEntity(entity);
            }

            Entity = entity;
        }

        public sealed override void TryAddComponent()
        {
            if (Entity.IsAlive() && !Entity.Has<T>())
                Entity.Replace(_component);
        }

        public sealed override void TryDeleteComponent()
        {
            if (Entity.IsAlive() && Entity.Has<T>())
                Entity.Del<T>();
        }

        public sealed override bool EntityHasComponent() => Entity.IsAlive() && Entity.Has<T>();

        protected virtual void PreInitializeEntity(EcsEntity entity) { }
        protected virtual void PostInitializeEntity(EcsEntity entity) { }
    }
}