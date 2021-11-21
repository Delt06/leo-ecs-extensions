using System;
using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Object = UnityEngine.Object;
#if LEOECS_EXTENSIONS_LITE
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;

#else
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
#endif

namespace DELTation.LeoEcsExtensions.Views
{
    public static class EcsEntityExtensions
    {
        public static void SetViewBackRefTyped<T>(this EcsPackedEntity entity, [NotNull] T view) where T : IEntityView
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            entity.GetCompatible<ViewBackRef<T>>().View = view;
        }

        public static void SetViewBackRef(this EcsPackedEntity entity, [NotNull] IEntityView view)
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            entity.GetCompatible<ViewBackRef>().View = view;
        }

        public static void SetUnityObjectData<T>(this EcsPackedEntity entity, [NotNull] T @object) where T : Object
        {
#if DEBUG
            if (!entity.IsAliveCompatible()) throw new ArgumentNullException(nameof(entity));
            if (@object == null) throw new ArgumentNullException(nameof(@object));
#endif

            entity.GetCompatible<UnityObjectData<T>>().Object = @object;
        }
    }
}