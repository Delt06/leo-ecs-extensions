using System;

namespace Runner.Movement
{
    [Serializable]
    public struct SidePosition
    {
        // [-1; 1]
        public float TargetPosition;
        public float CurrentPosition;
        public float CurrentVelocity;
    }
}