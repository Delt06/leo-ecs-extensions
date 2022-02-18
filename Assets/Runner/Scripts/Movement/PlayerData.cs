using System;
using UnityEngine;

namespace Runner.Movement
{
    [Serializable]
    public struct PlayerData
    {
        public float T;
        public float SidePositionNormalized; // [-1; 1]
        public Vector3 Forward;
    }
}