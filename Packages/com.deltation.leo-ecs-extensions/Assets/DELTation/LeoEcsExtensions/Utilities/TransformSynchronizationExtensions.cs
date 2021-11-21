using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Systems;
using JetBrains.Annotations;
#if LEOECS_EXTENSIONS_LITE
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using UnityEngine;
#else
using Leopotam.Ecs;
using DELTation.LeoEcsExtensions.Components;
using UnityEngine;
#endif

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class TransformSynchronizationExtensions
    {
#if !LEOECS_EXTENSIONS_LITE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdatePosition(this in EcsEntity entity, ref Position position, Vector3 newWorldPosition)
        {
            position.WorldPosition = newWorldPosition;
            entity.RequirePositionWrite();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateRotation(this in EcsEntity entity, ref Rotation rotation, Quaternion newWorldRotation)
        {
            rotation.WorldRotation = newWorldRotation;
            entity.RequireRotationWrite();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateScale(this in EcsEntity entity, ref Scale scale, Vector3 newLocalScale)
        {
            scale.LocalScale = newLocalScale;
            entity.RequireScaleWrite();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequirePositionWrite(this in EcsEntity entity) => entity.Get<PositionWriteRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireRotationWrite(this in EcsEntity entity) => entity.Get<RotationWriteRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireScaleWrite(this in EcsEntity entity) => entity.Get<ScaleWriteRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequirePositionRead(this in EcsEntity entity) => entity.Get<PositionReadRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireRotationRead(this in EcsEntity entity) => entity.Get<RotationReadRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireScaleRead(this in EcsEntity entity) => entity.Get<ScaleReadRequired>();

#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdatePosition([NotNull] this EcsPool<PositionWriteRequired> pool, in int entity,
            ref Position position, Vector3 newWorldPosition)
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif
            position.WorldPosition = newWorldPosition;
            pool.GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateRotation([NotNull] this EcsPool<RotationWriteRequired> pool, in int entity,
            ref Rotation rotation, Quaternion newWorldRotation)
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif
            rotation.WorldRotation = newWorldRotation;
            pool.GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateScale([NotNull] this EcsPool<RotationWriteRequired> pool, in int entity,
            ref Scale scale, Vector3 newLocalScale)
        {
#if DEBUG
            if (pool == null) throw new ArgumentNullException(nameof(pool));
#endif
            scale.LocalScale = newLocalScale;
            pool.GetOrAdd(entity);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequirePositionRead(this in EcsPackedEntityWithWorld entity) =>
            entity.GetOrAdd<PositionReadRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireRotationRead(this in EcsPackedEntityWithWorld entity) =>
            entity.GetOrAdd<RotationReadRequired>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireScaleRead(this in EcsPackedEntityWithWorld entity) =>
            entity.GetOrAdd<ScaleReadRequired>();

#endif

        [NotNull]
        public static EcsSystems ReadFromTransforms([NotNull] this EcsSystems systems)
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));
            return systems.Add(new ReadFromTransformsSystem());
        }

        [NotNull]
        public static EcsSystems WriteToTransforms([NotNull] this EcsSystems systems)
        {
            if (systems == null) throw new ArgumentNullException(nameof(systems));
            return systems.Add(new WriteToTransformsSystem());
        }
    }
}