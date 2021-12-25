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

            if (DIFramework.Di.TryResolveGlobally(out IActiveEcsWorld activeEcsWorld))
            {
                var world = activeEcsWorld.World;

                if (IsEcsWorld(type))
                {
                    dependency = world;
                    return true;
                }
            }


            dependency = default;
            return false;
        }

        public bool CanBeResolvedSafe(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return IsEcsWorld(type) &&
                   DIFramework.Di.CanBeResolvedGloballySafe<IActiveEcsWorld>();
        }

        public void GetAllRegisteredObjects(ICollection<object> objects) { }

        private static bool IsEcsWorld(Type type) => typeof(EcsWorld).IsAssignableFrom(type);
    }
}

#endif