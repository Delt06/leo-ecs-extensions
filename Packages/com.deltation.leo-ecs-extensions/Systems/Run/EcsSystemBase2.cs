using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.ExtendedPools;
using DELTation.LeoEcsExtensions.Utilities;
using DELTation.LeoEcsExtensions.Views;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public abstract class EcsSystemBase2 : IEcsPreInitSystem
    {
        public EcsWorld World
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public void PreInit(EcsSystems systems)
        {
            World = systems.GetWorld();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected EcsWorld.Mask Filter<T>() where T : struct => World.Filter<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected EcsWorld.Mask FilterOnUpdateOf<T>() where T : struct => World.FilterOnUpdateOf<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected EcsWorld.Mask FilterAndIncUpdateOf<T>() where T : struct => World.FilterAndIncUpdateOf<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected ref T Get<T>(int entity) where T : struct =>
            ref World.GetPool<T>().Get(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ref T GetOrAdd<T>(int entity) where T : struct =>
            ref World.GetPool<T>().GetOrAdd(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected ref readonly T Read<T>(int entity) where T : struct =>
            ref World.GetPool<T>().Get(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ref T Modify<T>(int entity) where T : struct
        {
            World.GetUpdatesPool<T>().GetOrAdd(entity);
            return ref World.GetPool<T>().Get(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ref T ModifyOrAdd<T>(int entity) where T : struct
        {
            World.GetUpdatesPool<T>().GetOrAdd(entity);
            return ref World.GetPool<T>().GetOrAdd(entity);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ref T Add<T>(int entity) where T : struct => ref World.GetPool<T>().Add(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Del<T>(int entity) where T : struct =>
            World.GetPool<T>().Del(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void DelModify<T>(int entity) where T : struct
        {
            World.GetPool<T>().Del(entity);
            World.GetUpdatesPool<T>().GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Destroy(int entity) =>
            World.DelEntity(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected bool Has<T>(int entity) where T : struct =>
            World.GetPool<T>().Has(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected Transform GetTransform(int entity) => Get<UnityRef<Transform>>(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected IEntityView GetView(int entity) => Get<ViewBackRef>(entity).View;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        protected TView GetView<TView>(int entity) =>
            Get<ViewBackRef<TView>>(entity);
    }
}