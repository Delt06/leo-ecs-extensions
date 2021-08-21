using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
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