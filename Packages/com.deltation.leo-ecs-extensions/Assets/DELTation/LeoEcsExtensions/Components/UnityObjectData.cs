using System;
using Object = UnityEngine.Object;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct UnityObjectData<T> where T : Object
    {
        public T Object;

        public static implicit operator T(UnityObjectData<T> data) => data.Object;
    }
}