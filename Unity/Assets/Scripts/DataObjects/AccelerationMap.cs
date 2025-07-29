

namespace DrivingGameV2 {

    public readonly struct AccelerationMap {
        public readonly float Forward;
        public readonly float Reverse;
        public readonly float Left;
        public readonly float Right;

        public AccelerationMap(float forward, float reverse, float left, float right) {
            this.Forward = forward;
            this.Reverse = reverse;
            this.Left = left;
            this.Right = right;
        }
    }
}
