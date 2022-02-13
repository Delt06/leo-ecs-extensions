using System;
using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Utilities;
using DELTation.LeoEcsExtensions.Views;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Pooling
{
    public class EntityViewPool : MonoBehaviour, IEntityViewPool
    {
        [SerializeField]
        private EntityView _prefab;
        [SerializeField] [Min(0)] private int _initialCapacity = 10;
        [SerializeField] private bool _createAsChildren = true;
        private readonly HashSet<EntityView> _allInstances = new HashSet<EntityView>();

        private readonly Queue<EntityView> _freeInstancesQueue = new Queue<EntityView>();
        private readonly HashSet<EntityView> _freeInstancesSet = new HashSet<EntityView>();
        private readonly Dictionary<EntityView, IEntityViewPoolingListener[]> _listeners =
            new Dictionary<EntityView, IEntityViewPoolingListener[]>();

        private void Awake()
        {
            for (var i = 0; i < _initialCapacity; i++)
            {
                CreateNewFree();
            }
        }

        public EntityView Create(Vector3 position, Quaternion rotation)
        {
            if (_freeInstancesSet.Count == 0) CreateNewFree();

            var instance = _freeInstancesQueue.Dequeue();
            _freeInstancesSet.Remove(instance);

            instance.transform.SetPositionAndRotation(position, rotation);

            foreach (var listener in _listeners[instance])
            {
                listener.OnPreCreated();
            }

            instance.gameObject.SetActive(true);
            instance.CreateEntity();
            var tempQualifier = instance.GetOrCreateEntity();
            tempQualifier.GetOrAdd<PoolBackRef>().Pool = this;

            return instance;
        }

        public void Dispose(EntityView instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (!_allInstances.Contains(instance)) return;
            if (_freeInstancesSet.Contains(instance)) return;

            foreach (var listener in _listeners[instance])
            {
                listener.OnPreDisposed();
            }

            instance.DestroyEntity();
            instance.gameObject.SetActive(false);
            _freeInstancesQueue.Enqueue(instance);
            _freeInstancesSet.Add(instance);
        }

        private void CreateNewFree()
        {
            var instance = _createAsChildren ? Instantiate(_prefab, transform) : Instantiate(_prefab);
            _listeners[instance] = instance.GetComponentsInChildren<IEntityViewPoolingListener>();
            _allInstances.Add(instance);
            Dispose(instance);
        }
    }
}