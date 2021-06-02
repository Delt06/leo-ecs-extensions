﻿using System;
using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Views;
using Leopotam.Ecs;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Pooling
{
    public class EntityViewPool : MonoBehaviour, IEntityViewPool
    {
        [SerializeField]
#if UNITY_EDITOR && ODIN_INSPECTOR
        [Required]
#endif
        private EntityView _prefab = default;
        [SerializeField] [Min(0)] private int _initialCapacity = 10;
        [SerializeField] private bool _createAsChildren = true;

#if UNITY_EDITOR && ODIN_INSPECTOR
        [HideInEditorMode] [ShowInInspector]
        private int CurrentCapacity => _allInstances.Count;
#endif

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
            instance.Entity.Get<PoolBackRef>().Pool = this;

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

        private void Awake()
        {
            for (var i = 0; i < _initialCapacity; i++)
            {
                CreateNewFree();
            }
        }

        private readonly Queue<EntityView> _freeInstancesQueue = new Queue<EntityView>();
        private readonly HashSet<EntityView> _freeInstancesSet = new HashSet<EntityView>();
        private readonly HashSet<EntityView> _allInstances = new HashSet<EntityView>();
        private readonly Dictionary<EntityView, IEntityViewPoolingListener[]> _listeners =
            new Dictionary<EntityView, IEntityViewPoolingListener[]>();
    }
}