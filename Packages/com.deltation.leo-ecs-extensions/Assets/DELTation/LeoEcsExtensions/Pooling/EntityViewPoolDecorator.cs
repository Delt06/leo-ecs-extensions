using DELTation.LeoEcsExtensions.Views;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Pooling
{
    [RequireComponent(typeof(EntityViewPool))]
    public abstract class EntityViewPoolDecorator<TView> : MonoBehaviour, IEntityViewPool, IEntityViewPool<TView>
        where TView : EntityView
    {
        public TView Create(Vector3 position, Quaternion rotation) =>
            (TView) ((IEntityViewPool) this).Create(position, rotation);

        EntityView IEntityViewPool.Create(Vector3 position, Quaternion rotation) => Pool.Create(position, rotation);

        public void Dispose(TView view) => ((IEntityViewPool) this).Dispose(view);

        void IEntityViewPool.Dispose(EntityView instance) => Pool.Dispose(instance);

        private EntityViewPool Pool => _pool ? _pool : _pool = GetComponent<EntityViewPool>();

        private EntityViewPool _pool;
    }
}