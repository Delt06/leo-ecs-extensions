#if LEOECS_EXTENSIONS_LITE

using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public sealed class EcsPackedFilter
    {
        private readonly EcsFilter _filter;

        public EcsPackedFilter(EcsFilter filter) => _filter = filter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsWorld GetWorld() => _filter.GetWorld();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetEntitiesCount() => _filter.GetEntitiesCount();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter GetInternalFilter() => _filter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(_filter.GetEnumerator(), _filter.GetWorld());

        public struct Enumerator : IDisposable
        {
            private EcsFilter.Enumerator _internalEnumerator;
            private readonly EcsWorld _world;

            internal Enumerator(EcsFilter.Enumerator internalEnumerator, EcsWorld world)
            {
                _internalEnumerator = internalEnumerator;
                _world = world;
            }

            public EcsPackedEntityWithWorld Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _world.PackEntityWithWorld(_internalEnumerator.Current);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => _internalEnumerator.MoveNext();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose() => _internalEnumerator.Dispose();
        }
    }
}

#endif