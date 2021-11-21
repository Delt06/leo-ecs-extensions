using System;
using Object = UnityEngine.Object;
#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct UnityObjectData<T> : IEcsAutoReset<UnityObjectData<T>> where T : Object
    {
        public T Object;

        public void AutoReset(ref UnityObjectData<T> c)
        {
            c.Object = default;
        }

        public static implicit operator T(UnityObjectData<T> data) => data.Object;
    }
}