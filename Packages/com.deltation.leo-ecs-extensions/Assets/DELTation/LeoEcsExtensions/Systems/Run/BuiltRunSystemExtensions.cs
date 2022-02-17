using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public static class BuiltRunSystemExtensions
    {
        [MustUseReturnValue] [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BuiltRunSystem Map([NotNull] EcsWorld.Mask mask, [NotNull] Delegate action)
        {
#if DEBUG
            if (mask == null) throw new ArgumentNullException(nameof(mask));
            if (action == null) throw new ArgumentNullException(nameof(action));
#endif
            return new BuiltRunSystem(mask.End(), action);
        }

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1>(this EcsWorld.Mask mask,
            Action<T1> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2>(this EcsWorld.Mask mask,
            Action<T1, T2> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2, T3>(this EcsWorld.Mask mask,
            Action<T1, T2, T3> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2, T3, T4>(this EcsWorld.Mask mask,
            Action<T1, T2, T3, T4> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2, T3, T4, T5>(this EcsWorld.Mask mask,
            Action<T1, T2, T3, T4, T5> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2, T3, T4, T5, T6>(this EcsWorld.Mask mask,
            Action<T1, T2, T3, T4, T5, T6> action) =>
            Map(mask, action);

        [MustUseReturnValue]
        public static BuiltRunSystem MapTo<T1, T2, T3, T4, T5, T6, T7>(this EcsWorld.Mask mask,
            Action<T1, T2, T3, T4, T5, T6, T7> action) =>
            Map(mask, action);
    }
}