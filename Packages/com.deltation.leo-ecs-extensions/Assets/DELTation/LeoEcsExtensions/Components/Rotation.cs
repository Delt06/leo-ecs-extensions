using System;
using UnityEngine;
#if !LEOECS_EXTENSIONS_LITE
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Rotation
    {
        public Quaternion WorldRotation;
    }

    public struct RotationWriteRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }

    public struct RotationReadRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }
}