using UnityEngine;

namespace TriangleTT {
    public static class FpsDisplayManager {
        public static void Update() {
            SceneObjects.FpsTextLabel.text = $"FPS: {1f / Time.deltaTime}";
        }
    }
}
