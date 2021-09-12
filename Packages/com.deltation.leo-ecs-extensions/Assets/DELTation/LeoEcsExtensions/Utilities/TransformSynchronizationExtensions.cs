using System;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Systems;
using JetBrains.Annotations;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class TransformSynchronizationExtensions
    {
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