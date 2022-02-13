using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    public readonly struct EcsReadOnlyPool<T> : IEnumerable<T> where T : struct
    {
        private readonly EcsPool<T> _pool;

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
    }
}