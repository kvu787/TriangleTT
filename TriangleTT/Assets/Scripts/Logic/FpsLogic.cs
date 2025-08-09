using UnityEngine;

namespace TriangleTT {
    public static class FpsLogic {
        public static void Update() {
            SceneObjects.FpsTextLabel.text = $"FPS: {1f / Time.deltaTime}";
        }
    }
}
