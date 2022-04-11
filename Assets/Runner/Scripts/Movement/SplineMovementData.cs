using System;
using UnityEngine;

namespace Runner.Movement
{
    [Serializable]
    public struct SplineMovementData
    {
        public float T;
        public Vector3 Forward;
    }
}