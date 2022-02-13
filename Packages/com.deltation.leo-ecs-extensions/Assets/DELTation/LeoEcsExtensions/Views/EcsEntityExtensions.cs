using System;
using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Object = UnityEngine.Object;

namespace DELTation.LeoEcsExtensions.Views
{
    public static class EcsEntityExtensions
    {
        public static void SetViewBackRefTyped<T>(this EcsPackedEntityWithWorld entity, [NotNull] T view)
            where T : IEntityView
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            entity.GetCompatible<ViewBackRef<T>>().View = view;
        }

        public static void SetViewBackRef(this EcsPackedEntityWithWorld entity, [NotNull] IEntityView view)
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            entity.GetCompatible<ViewBackRef>().View = view;
        }

        public static void SetUnityObjectData<T>(this EcsPackedEntityWithWorld entity, [NotNull] T @object)
            where T : Object
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (@object == null) throw new ArgumentNullException(nameof(@object));
#endif

            entity.GetCompatible<UnityObjectData<T>>().Object = @object;
        }
    }
}