using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using Leopotam.Ecs;
using UnityEngine;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public class WorldDependencyContainer : MonoBehaviour, IDependencyContainer
    {
        [SerializeField]
#if UNITY_EDITOR && ODIN_INSPECTOR
        [Required]
#endif
        private EcsEntryPoint _ecsEntryPoint;

        public bool TryResolve(Type type, out object dependency)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!CanBeResolvedSafe(type))
            {
                dependency = default;
                return false;
            }

            dependency = _ecsEntryPoint.World;
            return true;
        }

        public bool CanBeResolvedSafe(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return typeof(EcsWorld).IsAssignableFrom(type);
        }

        public void GetAllRegisteredObjects(ICollection<object> objects) { }
    }
}