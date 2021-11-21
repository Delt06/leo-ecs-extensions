#if !LEOECS_EXTENSIONS_LITE
using Leopotam.Ecs;
#endif
using System;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Position
    {
        public Vector3 WorldPosition;
    }

    public struct PositionWriteRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }

    public struct PositionReadRequired
#if !LEOECS_EXTENSIONS_LITE
        : IEcsIgnoreInFilter
#endif
    { }
}