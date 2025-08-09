using UnityEngine;

namespace TriangleTT {
    public static class MaxQueuedFramesManager {
        public static void Init() {
            QualitySettings.maxQueuedFrames = SceneSettings.MaxQueuedFrames;
        }
    }
}
