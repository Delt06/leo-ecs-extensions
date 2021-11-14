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
        public abstract void TryUpdateStoredValueFromEntity();
        public abstract void TryUpdateEntityFromStoredValue();
    }

    public abstract class ComponentView<T> : ComponentView, IEntityInitializer where T : struct
    {
        [SerializeField] private bool _enabled = true;

        [SerializeField] private T _component;

        public ref T StoredComponentValue => ref _component;

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

        public override void TryUpdateEntityFromStoredValue()
        {
            if (!Entity.IsAlive()) return;
            if (!Entity.Has<T>()) return;

            ref var attachedComponent = ref Entity.Get<T>();
            if (!StoredComponentEquals(attachedComponent))
                Entity.Replace(_component);
        }

        private bool StoredComponentEquals(in T other) => Equals(_component, other);

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

        public sealed override void TryUpdateStoredValueFromEntity()
        {
            if (!Entity.IsAlive()) return;
            if (!Entity.Has<T>()) return;

            ref var attachedComponent = ref Entity.Get<T>();
            if (!StoredComponentEquals(attachedComponent))
                _component = attachedComponent;
        }

        public sealed override bool EntityHasComponent() => Entity.IsAlive() && Entity.Has<T>();

        protected virtual void PreInitializeEntity(EcsEntity entity) { }
        protected virtual void PostInitializeEntity(EcsEntity entity) { }
    }
}