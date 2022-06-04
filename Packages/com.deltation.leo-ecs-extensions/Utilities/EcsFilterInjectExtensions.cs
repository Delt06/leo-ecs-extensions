#if LEOECS_DI
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public static class EcsFilterInjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool IsEmpty<TInc>(this EcsFilterInject<TInc> filter) where TInc : struct, IEcsInclude =>
            filter.Value.IsEmpty();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool IsEmpty<TInc, TExc>(this EcsFilterInject<TInc, TExc> filter) where TInc : struct, IEcsInclude
            where TExc : struct, IEcsExclude =>
            filter.Value.IsEmpty();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static EcsFilter.Enumerator GetEnumerator<TInc>(this EcsFilterInject<TInc> filter)
            where TInc : struct, IEcsInclude =>
            filter.Value.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static EcsFilter.Enumerator GetEnumerator<TInc, TExc>(this EcsFilterInject<TInc, TExc> filter)
            where TInc : struct, IEcsInclude
            where TExc : struct, IEcsExclude =>
            filter.Value.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc>(this EcsFilterInject<TInc> filter, int entity)
            where TInc : struct, IEcsInclude =>
            filter.Value.Contains(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc, TExc>(this EcsFilterInject<TInc, TExc> filter, int entity)
            where TInc : struct, IEcsInclude
            where TExc : struct, IEcsExclude =>
            filter.Value.Contains(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc>(this EcsFilterInject<TInc> filter, EcsPackedEntityWithWorld packedEntity)
            where TInc : struct, IEcsInclude =>
            filter.Value.Contains(packedEntity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc, TExc>(this EcsFilterInject<TInc, TExc> filter,
            EcsPackedEntityWithWorld packedEntity)
            where TInc : struct, IEcsInclude
            where TExc : struct, IEcsExclude =>
            filter.Value.Contains(packedEntity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc>(this EcsFilterInject<TInc> filter, EcsPackedEntityWithWorld packedEntity,
            out int entity)
            where TInc : struct, IEcsInclude =>
            filter.Value.Contains(packedEntity, out entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public static bool Contains<TInc, TExc>(this EcsFilterInject<TInc, TExc> filter,
            EcsPackedEntityWithWorld packedEntity, out int entity)
            where TInc : struct, IEcsInclude
            where TExc : struct, IEcsExclude =>
            filter.Value.Contains(packedEntity, out entity);
    }
}

#endif