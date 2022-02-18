using System;
using UnityEngine;

namespace Runner.Movement
{
    [Serializable]
    public struct PlayerData
    {
        public float T;
        [Min(0f)]
        public float Speed;

        public float SidePositionNormalized; // [-1; 1]
        public Vector3 Forward;
    }
}