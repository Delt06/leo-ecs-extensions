#if LEOECS_EXTENSIONS_LITE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Utilities
{
    public readonly struct EcsObservablePool<T> : IEnumerable<T> where T : struct
    {
        private readonly EcsPool<T> _pool;
        private readonly EcsPool<UpdateEvent<T>> _updatePool;

        internal EcsObservablePool([NotNull] EcsPool<T> pool, [NotNull] EcsPool<UpdateEvent<T>> updatePool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _updatePool = updatePool ?? throw new ArgumentNullException(nameof(updatePool));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entity) => _pool.Has(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del(int entity)
        {
            if (Has(entity))
                OnChanged(entity);
            _pool.Del(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add(int entity)
        {
            OnChanged(entity);
            return ref _pool.Add(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Modify(int entity)
        {
#if DEBUG
            EnsureHas(entity);
#endif
            OnChanged(entity);
            return ref _pool.Get(entity);
        }

        private void OnChanged(int entity)
        {
            _updatePool.GetOrAdd(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureHas(int entity)
        {
            if (!Has(entity))
                throw new ArgumentException($"Entity does not have a {typeof(T)} component.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T Read(int entity)
        {
#if DEBUG
            EnsureHas(entity);
#endif
            return ref _pool.Get(entity);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) _pool.GetRawDenseItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();
    }
}
#endif