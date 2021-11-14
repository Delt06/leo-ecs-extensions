using System;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DELTation.LeoEcsExtensions.Views
{
    public static class EcsEntityExtensions
    {
        public static void SetViewBackRefTyped<T>(this EcsEntity entity, [NotNull] T view) where T : IEntityView
        {
#if DEBUG
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            ref var viewBackRef = ref entity.Get<ViewBackRef<T>>();
            viewBackRef.View = view;
        }

        public static void SetViewBackRef(this EcsEntity entity, [NotNull] IEntityView view)
        {
#if DEBUG
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (view == null) throw new ArgumentNullException(nameof(view));
#endif

            ref var viewBackRef = ref entity.Get<ViewBackRef>();
            viewBackRef.View = view;
        }

        public static void SetUnityObjectData<T>(this EcsEntity entity, [NotNull] T @object) where T : Object
        {
#if DEBUG
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (@object == null) throw new ArgumentNullException(nameof(@object));
#endif

            ref var unityObjectData = ref entity.Get<UnityObjectData<T>>();
            unityObjectData.Object = @object;
        }

        public static void SetTransformComponentsFromTransform(this EcsEntity entity, [NotNull] Transform transform)
        {
#if DEBUG
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (transform == null) throw new ArgumentNullException(nameof(transform));
#endif

            entity.Get<Position>().WorldPosition = transform.position;
            entity.Get<Rotation>().WorldRotation = transform.rotation;
            entity.Get<Scale>().LocalScale = transform.localScale;
        }
    }
}