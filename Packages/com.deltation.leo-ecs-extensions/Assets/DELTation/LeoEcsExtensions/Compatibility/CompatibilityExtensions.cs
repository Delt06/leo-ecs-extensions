using System;
using System.Runtime.CompilerServices;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;
using EcsPackedEntity = Leopotam.EcsLite.EcsPackedEntityWithWorld;
using EcsWorld = Leopotam.EcsLite.EcsWorld;

#else
using Leopotam.Ecs;
using EcsPackedEntity = Leopotam.Ecs.EcsEntity;
using EcsWorld = Leopotam.Ecs.EcsWorld;
#endif

namespace DELTation.LeoEcsExtensions.Compatibility
{
    internal static class EcsEntityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAliveCompatible(this EcsPackedEntity entity)
        {
#if LEOECS_EXTENSIONS_LITE
            return entity.Unpack(out _, out _);
#else
            return entity.IsAlive();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntity NewPackedEntityCompatible(this EcsWorld world)
        {
#if LEOECS_EXTENSIONS_LITE
            var entity = world.NewEntity();
            return world.PackEntityWithWorld(entity);
#else
            return world.NewEntity();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetCompatible<T>(this in EcsPackedEntity entity) where T : struct
        {
#if LEOECS_EXTENSIONS_LITE
            entity.Unpack(out var world, out var entityIdx);
            var pool = world.GetPool<T>();
            if (!pool.Has(entityIdx))
                return ref pool.Add(entityIdx);
            return ref pool.Get(entityIdx);

#else
            return ref entity.Get<T>();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasCompatible<T>(this EcsPackedEntity entity) where T : struct
        {
#if LEOECS_EXTENSIONS_LITE
            return entity.Unpack(out var world, out var entityIdx) &&
                   world.GetPool<T>().Has(entityIdx);

#else
            return entity.Has<T>();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelCompatible<T>(this EcsPackedEntity entity) where T : struct
        {
#if LEOECS_EXTENSIONS_LITE
            entity.Unpack(out var world, out var entityIdx);
            world.GetPool<T>().Del(entityIdx);

#else
            entity.Del<T>();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyCompatible(this EcsPackedEntity entity)
        {
#if LEOECS_EXTENSIONS_LITE
            entity.Unpack(out var world, out var entityIdx);
            world.DelEntity(entityIdx);
#else
            entity.Destroy();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetComponentTypesCompatible(this EcsPackedEntity entity)
        {
            Type[] componentTypes = null;
#if LEOECS_EXTENSIONS_LITE

            entity.Unpack(out var world, out var entityIdx);
            world.GetComponentTypes(entityIdx, ref componentTypes);
#else
            entity.GetComponentTypes(ref componentTypes);
#endif
            return componentTypes;
        }
    }
}