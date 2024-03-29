﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    [EcsPoolAttribute(nameof(CreateInstance), nameof(GetComponentType))]
    public readonly struct EcsObservablePool<T> : IEnumerable<T>, IEquatable<EcsObservablePool<T>> where T : struct
    {
        private readonly EcsPool<T> _pool;
        private readonly EcsPool<UpdateEvent<T>> _updatePool;

        private static EcsObservablePool<T> CreateInstance(EcsWorld world)
        {
            var pool = world.GetPool<T>();
            var updateEventPool = world.GetPool<UpdateEvent<T>>();
            return new EcsObservablePool<T>(pool, updateEventPool);
        }

        private static Type GetComponentType() => typeof(T);

        internal EcsObservablePool([NotNull] EcsPool<T> pool, [NotNull] EcsPool<UpdateEvent<T>> updatePool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _updatePool = updatePool ?? throw new ArgumentNullException(nameof(updatePool));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
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
        [MustUseReturnValue]
        public ref readonly T Read(int entity)
        {
#if DEBUG
            EnsureHas(entity);
#endif
            return ref _pool.Get(entity);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) _pool.GetRawDenseItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>) this).GetEnumerator();

        public bool Equals(EcsObservablePool<T> other) =>
            Equals(_pool, other._pool) && Equals(_updatePool, other._updatePool);

        public override bool Equals(object obj) => obj is EcsObservablePool<T> other && Equals(other);

        public override int GetHashCode() => _pool?.GetHashCode() ?? 0 ^ _updatePool?.GetHashCode() ?? 0;
    }
}