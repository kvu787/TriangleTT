using System;

namespace TriangleTT {
    [Serializable]
    public struct AccelerationMap {
        public float Forward;
        public float Reverse;
        public float Left;
        public float Right;

        public AccelerationMap(float forward, float reverse, float left, float right) {
            this.Forward = forward;
            this.Reverse = reverse;
            this.Left = left;
            this.Right = right;
        }
    }
}
