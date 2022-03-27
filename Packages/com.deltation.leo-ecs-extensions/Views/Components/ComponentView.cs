#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using DELTation.LeoEcsExtensions.Views.Components.Attributes;
#endif
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components
{
    public abstract class ComponentView : MonoBehaviour
    {
        public EcsPackedEntityWithWorld Entity { get; protected set; }

        internal abstract void TryAddComponent();
        internal abstract void TryDeleteComponent();
        internal abstract bool EntityHasComponent();
        internal abstract void TryUpdateDisplayedValueFromEntity();
        internal abstract void TryUpdateEntityFromDisplayedValue();
    }

    public abstract class ComponentView<T> : ComponentView, IEntityInitializer where T : struct
    {
        [SerializeField] private bool _enabled = true;

        [SerializeField]
#if ODIN_INSPECTOR
        [HideIf(nameof(EntityHasComponent))]
        [InlineProperty]
        [HideLabel]
        [Header("Component")]
#else
        [HideIfEntityHasComponent]
#endif
        private T _component;

        [SerializeField]
#if ODIN_INSPECTOR
        [ShowIf(nameof(EntityHasComponent))]
#else
        [ShowIfEntityHasComponent]
#endif
        private T _displayedComponent;

        public ref T StoredComponentValue => ref _component;

        public void InitializeEntity(EcsPackedEntityWithWorld entity)
        {
            if (_enabled)
            {
                PreInitializeEntity(entity);
                entity.GetOrAdd<T>() = _component;
                PostInitializeEntity(entity);
            }

            Entity = entity;
        }

        internal override void TryUpdateEntityFromDisplayedValue()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetOrAdd<T>();
            if (!DisplayedComponentEquals(attachedComponent))
                Entity.GetOrAdd<T>() = _displayedComponent;
        }

        private bool DisplayedComponentEquals(in T other) => Equals(_displayedComponent, other);

        internal sealed override void TryAddComponent()
        {
            if (Entity.IsAlive() && !Entity.Has<T>())
                Entity.GetOrAdd<T>() = _component;
        }

        internal sealed override void TryDeleteComponent()
        {
            if (Entity.Has<T>())
                Entity.Del<T>();
        }

        internal sealed override void TryUpdateDisplayedValueFromEntity()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetOrAdd<T>();
            if (!DisplayedComponentEquals(attachedComponent))
                _displayedComponent = attachedComponent;
        }

        internal sealed override bool EntityHasComponent() => Entity.IsAlive() && Entity.Has<T>();

        protected virtual void PreInitializeEntity(EcsPackedEntityWithWorld entity) { }
        protected virtual void PostInitializeEntity(EcsPackedEntityWithWorld entity) { }
    }
}