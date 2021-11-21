using DELTation.LeoEcsExtensions.Compatibility;
using UnityEngine;
#if LEOECS_EXTENSIONS_LITE
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class ComponentView : MonoBehaviour
    {
        public EcsPackedEntity Entity { get; protected set; }

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

        public void InitializeEntity(EcsPackedEntity entity)
        {
            if (_enabled)
            {
                PreInitializeEntity(entity);
                entity.GetCompatible<T>() = _component;
                PostInitializeEntity(entity);
            }

            Entity = entity;
        }

        public override void TryUpdateEntityFromStoredValue()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetCompatible<T>();
            if (!StoredComponentEquals(attachedComponent))
                Entity.GetCompatible<T>() = _component;
        }

        private bool StoredComponentEquals(in T other) => Equals(_component, other);

        public sealed override void TryAddComponent()
        {
            if (Entity.IsAliveCompatible() && !Entity.HasCompatible<T>())
                Entity.GetCompatible<T>() = _component;
        }

        public sealed override void TryDeleteComponent()
        {
            if (Entity.HasCompatible<T>())
                Entity.DelCompatible<T>();
        }

        public sealed override void TryUpdateStoredValueFromEntity()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetCompatible<T>();
            if (!StoredComponentEquals(attachedComponent))
                _component = attachedComponent;
        }

        public sealed override bool EntityHasComponent() => Entity.HasCompatible<T>();

        protected virtual void PreInitializeEntity(EcsPackedEntity entity) { }
        protected virtual void PostInitializeEntity(EcsPackedEntity entity) { }
    }
}