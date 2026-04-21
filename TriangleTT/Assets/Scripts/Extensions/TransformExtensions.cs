using UnityEngine;
using UnityEngine.Assertions;

namespace TriangleTT {
    public static class TransformExtensions {
        public static void SetFrom(this Transform self, Transform other) {
            Assert.IsNotNull(self);
            Assert.IsNotNull(other);
            self.SetPositionAndRotation(other.position, other.rotation);
            self.localScale = other.localScale;
        }
    }
}
