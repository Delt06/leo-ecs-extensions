using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    [EcsPoolAttribute(nameof(CreateInstance), nameof(GetComponentType), IgnoredByDefault = true)]
    public readonly struct EcsSingletonPool<T> : IEnumerable<T>, IEquatable<EcsSingletonPool<T>> where T : struct
    {
        private readonly EcsFilter _filter;
        private readonly EcsPool<T> _pool;

        private static EcsSingletonPool<T> CreateInstance(EcsWorld world)
        {
            var filter = world.Filter<T>().End();
            var pool = world.GetPool<T>();
            return new EcsSingletonPool<T>(filter, pool);
        }

        private static Type GetComponentType() => typeof(T);

        internal EcsSingletonPool([NotNull] EcsFilter filter, [NotNull] EcsPool<T> pool)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public bool IsEmpty() => _filter.IsEmpty();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public ref T Get()
        {
            var i = _filter.GetSingle();
            return ref _pool.Get(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public int GetEntity() => _filter.GetSingle();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) _pool.GetRawDenseItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();

        public bool Equals(EcsSingletonPool<T> other) => Equals(_filter, other._filter) && Equals(_pool, other._pool);

        public override bool Equals(object obj) => obj is EcsSingletonPool<T> other && Equals(other);

        public override int GetHashCode() => _pool?.GetHashCode() ?? 0 ^ _filter?.GetHashCode() ?? 0;
    }
}