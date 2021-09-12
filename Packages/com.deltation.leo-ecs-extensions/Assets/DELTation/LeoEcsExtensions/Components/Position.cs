using System;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Position
    {
        public Vector3 WorldPosition;
    }

    public struct PositionWriteRequired : IEcsIgnoreInFilter { }

    public struct PositionReadRequired : IEcsIgnoreInFilter { }
}