using System;
using Object = UnityEngine.Object;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct UnityRef<T> where T : Object
    {
        public T Object;

        public static implicit operator T(UnityRef<T> data) => data.Object;
    }
}