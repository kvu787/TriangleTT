using UnityEngine;

namespace DrivingGameV2 {
    public class SceneSettings : MonoBehaviour {
        public static int MaxQueuedFrames;
        public int _MaxQueuedFrames;

        public static int VSyncCount;
        public int _VSyncCount;

        public static bool EnableRuntimeVSyncControl;
        public bool _EnableRuntimeVSyncControl;

        void Awake() {
            MaxQueuedFrames = this._MaxQueuedFrames;
            VSyncCount = this._VSyncCount;
            EnableRuntimeVSyncControl = this._EnableRuntimeVSyncControl;
        }
    }
}
