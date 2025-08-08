using UnityEngine;

namespace DrivingGameV2 {
    public static class MaxQueuedFramesLogic {
        public static void Init() {
            QualitySettings.maxQueuedFrames = SceneSettings.MaxQueuedFrames;
        }
    }
}
