using UnityEngine;

namespace TriangleTT {
    public static class MaxQueuedFramesLogic {
        public static void Init() {
            QualitySettings.maxQueuedFrames = SceneSettings.MaxQueuedFrames;
        }
    }
}
