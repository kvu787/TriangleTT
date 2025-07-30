using System;

namespace DrivingGameV2 {
    [Serializable]
    public struct Dynamic {
        public float VelocityLimiter;
        public AccelerationMap AccelerationMap; // Accel = left stick, Brake = right bumper

        //public object DriftMatrix;              // X 
        //public object BoostCore;                // Y
        //public object SuperBrake;               // B
        //public object JumpHydraulics;           // A

        public Dynamic(float velocityLimiter, AccelerationMap accelerationMap) {
            this.VelocityLimiter = velocityLimiter;
            this.AccelerationMap = accelerationMap;
        }
    }
}
