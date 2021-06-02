using System;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Rotation
    {
        public Quaternion WorldRotation;
    }

    public struct RotationWriteRequired : IEcsIgnoreInFilter { }

    public struct RotationReadRequired : IEcsIgnoreInFilter { }
}