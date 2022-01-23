using DELTation.LeoEcsExtensions.Compatibility;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using DELTation.LeoEcsExtensions.Views.Components.Attributes;
#endif

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

        internal override void TryUpdateEntityFromDisplayedValue()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetCompatible<T>();
            if (!DisplayedComponentEquals(attachedComponent))
                Entity.GetCompatible<T>() = _displayedComponent;
        }

        private bool DisplayedComponentEquals(in T other) => Equals(_displayedComponent, other);

        internal sealed override void TryAddComponent()
        {
            if (Entity.IsAliveCompatible() && !Entity.HasCompatible<T>())
                Entity.GetCompatible<T>() = _component;
        }

        internal sealed override void TryDeleteComponent()
        {
            if (Entity.HasCompatible<T>())
                Entity.DelCompatible<T>();
        }

        internal sealed override void TryUpdateDisplayedValueFromEntity()
        {
            if (!EntityHasComponent()) return;

            ref var attachedComponent = ref Entity.GetCompatible<T>();
            if (!DisplayedComponentEquals(attachedComponent))
                _displayedComponent = attachedComponent;
        }

        internal sealed override bool EntityHasComponent() => Entity.HasCompatible<T>();

        protected virtual void PreInitializeEntity(EcsPackedEntity entity) { }
        protected virtual void PostInitializeEntity(EcsPackedEntity entity) { }
    }
}