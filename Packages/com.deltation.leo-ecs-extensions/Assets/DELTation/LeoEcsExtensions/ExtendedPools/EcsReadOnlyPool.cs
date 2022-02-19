using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    [EcsPoolAttribute(nameof(CreateInstance), nameof(GetComponentType))]
    public readonly struct EcsReadOnlyPool<T> : IEnumerable<T>, IEquatable<EcsReadOnlyPool<T>> where T : struct
    {
        private readonly EcsPool<T> _pool;

        private static EcsReadOnlyPool<T> CreateInstance(EcsWorld world)
        {
            var pool = world.GetPool<T>();
            return new EcsReadOnlyPool<T>(pool);
        }

        private static Type GetComponentType() => typeof(T);

        internal EcsReadOnlyPool([NotNull] EcsPool<T> pool) =>
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entity) => _pool.Has(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T Read(int entity)
        {
#if DEBUG
            EnsureHas(entity);
#endif
            return ref _pool.Get(entity);
        }

        private void EnsureHas(int entity)
        {
            if (!Has(entity))
                throw new ArgumentException($"Entity does not have a {typeof(T)} component.");
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) _pool.GetRawDenseItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();

        public bool Equals(EcsReadOnlyPool<T> other) => Equals(_pool, other._pool);

        public override bool Equals(object obj) => obj is EcsReadOnlyPool<T> other && Equals(other);

        public override int GetHashCode() => _pool != null ? _pool.GetHashCode() : 0;
    }
}