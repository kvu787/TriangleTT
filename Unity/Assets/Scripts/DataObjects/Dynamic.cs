namespace DrivingGameV2 {
    public readonly struct Dynamic {
        public readonly float VelocityLimiter;
        public readonly AccelerationMap AccelerationMap; // Accel = left stick, Brake = right bumper

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
