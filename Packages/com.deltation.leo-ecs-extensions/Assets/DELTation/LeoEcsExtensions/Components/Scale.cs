using System;
using UnityEngine;
#if !LEOECS_EXTENSIONS_LITE
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Scale
    {
        public Vector3 LocalScale;
    }

    public struct ScaleWriteRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }

    public struct ScaleReadRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }
}