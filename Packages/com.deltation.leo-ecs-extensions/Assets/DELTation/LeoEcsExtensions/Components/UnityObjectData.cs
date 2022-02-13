using System;
using Leopotam.EcsLite;
using Object = UnityEngine.Object;

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