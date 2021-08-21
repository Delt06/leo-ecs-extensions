using System;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct Scale
    {
        public Vector3 LocalScale;
    }

    public struct ScaleWriteRequired : IEcsIgnoreInFilter { }

    public struct ScaleReadRequired : IEcsIgnoreInFilter { }
}