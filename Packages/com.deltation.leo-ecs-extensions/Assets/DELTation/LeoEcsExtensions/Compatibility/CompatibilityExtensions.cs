using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Compatibility
{
    internal static class EcsEntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAliveCompatible(this EcsPackedEntityWithWorld entity) => entity.Unpack(out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntityWithWorld NewPackedEntityCompatible(this EcsWorld world)
        {
            var entity = world.NewEntity();
            return world.PackEntityWithWorld(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetCompatible<T>(this in EcsPackedEntityWithWorld entity) where T : struct
        {
            entity.Unpack(out var world, out var entityIdx);
            var pool = world.GetPool<T>();
            if (!pool.Has(entityIdx))
                return ref pool.Add(entityIdx);
            return ref pool.Get(entityIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasCompatible<T>(this EcsPackedEntityWithWorld entity) where T : struct =>
            entity.Unpack(out var world, out var entityIdx) &&
            world.GetPool<T>().Has(entityIdx);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelCompatible<T>(this EcsPackedEntityWithWorld entity) where T : struct
        {
            entity.Unpack(out var world, out var entityIdx);
            world.GetPool<T>().Del(entityIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyCompatible(this EcsPackedEntityWithWorld entity)
        {
            entity.Unpack(out var world, out var entityIdx);
            world.DelEntity(entityIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetComponentTypesCompatible(this EcsPackedEntityWithWorld entity)
        {
            Type[] componentTypes = null;
            entity.Unpack(out var world, out var entityIdx);
            world.GetComponentTypes(entityIdx, ref componentTypes);
            return componentTypes;
        }
    }
}