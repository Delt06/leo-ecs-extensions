using System;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct UnityRef<T> where T : Object
    {
#if ODIN_INSPECTOR
        [Required]
#endif
        public T Object;

        public static implicit operator T(UnityRef<T> data) => data.Object;
    }
}