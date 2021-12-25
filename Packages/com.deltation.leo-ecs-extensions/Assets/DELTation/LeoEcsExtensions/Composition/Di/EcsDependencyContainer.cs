#if LEOECS_EXTENSIONS_LITE
using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using DELTation.LeoEcsExtensions.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    public class EcsDependencyContainer : MonoBehaviour, IDependencyContainer
    {
        public bool TryResolve(Type type, out object dependency)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!CanBeResolvedSafe(type))
            {
                dependency = default;
                return false;
            }

            if (!DIFramework.Di.TryResolveGlobally(out IActiveEcsWorld activeEcsWorld))
            {
                dependency = default;
                return false;
            }

            dependency = activeEcsWorld.World;
            return true;
        }

        public bool CanBeResolvedSafe(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return typeof(EcsWorld).IsAssignableFrom(type) &&
                   DIFramework.Di.CanBeResolvedGloballySafe<IActiveEcsWorld>();
        }

        public void GetAllRegisteredObjects(ICollection<object> objects) { }
    }
}

#endif