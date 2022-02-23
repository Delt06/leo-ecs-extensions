using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;
using TransformData = DELTation.LeoEcsExtensions.Components.UnityRef<UnityEngine.Transform>;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    [EcsPool(nameof(CreateInstance), nameof(GetComponentType))]
    public readonly struct EcsTransformPool : IEnumerable<TransformData>, IEquatable<EcsTransformPool>
    {
        private readonly EcsPool<TransformData> _pool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public bool Has(int entity) => _pool.Has(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MustUseReturnValue]
        public Transform Read(int entity)
        {
#if DEBUG
            EnsureHas(entity);
#endif
            return _pool.Get(entity).Object;
        }

        private void EnsureHas(int entity)
        {
            if (!Has(entity))
                throw new ArgumentException($"Entity does not have a {typeof(TransformData)} component.");
        }

        internal EcsTransformPool([NotNull] EcsPool<TransformData> pool) =>
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));

        private static EcsTransformPool CreateInstance(EcsWorld world)
        {
            var pool = world.GetPool<TransformData>();
            return new EcsTransformPool(pool);
        }

        private static Type GetComponentType() => typeof(TransformData);

        IEnumerator<TransformData> IEnumerable<TransformData>.GetEnumerator() =>
            (IEnumerator<TransformData>) _pool.GetRawDenseItems().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TransformData>) this).GetEnumerator();

        public bool Equals(EcsTransformPool other) => Equals(_pool, other._pool);

        public override bool Equals(object obj) => obj is EcsTransformPool other && Equals(other);

        public override int GetHashCode() => _pool != null ? _pool.GetHashCode() : 0;
    }
}