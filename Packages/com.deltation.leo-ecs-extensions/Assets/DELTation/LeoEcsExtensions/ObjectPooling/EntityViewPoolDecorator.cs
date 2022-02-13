using DELTation.LeoEcsExtensions.Views;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.ObjectPooling
{
    [RequireComponent(typeof(EntityViewPool))]
    public abstract class EntityViewPoolDecorator<TView> : MonoBehaviour, IEntityViewPool, IEntityViewPool<TView>
        where TView : EntityView
    {
        private EntityViewPool _pool;

        private EntityViewPool Pool => _pool ? _pool : _pool = GetComponent<EntityViewPool>();

        EntityView IEntityViewPool.Create(Vector3 position, Quaternion rotation) => Pool.Create(position, rotation);

        void IEntityViewPool.Dispose(EntityView instance) => Pool.Dispose(instance);

        public TView Create(Vector3 position, Quaternion rotation) =>
            (TView) ((IEntityViewPool) this).Create(position, rotation);

        public void Dispose(TView view) => ((IEntityViewPool) this).Dispose(view);
    }
}