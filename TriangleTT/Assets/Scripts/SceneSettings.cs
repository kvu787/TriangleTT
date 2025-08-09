using UnityEngine;

namespace TriangleTT {
    public class SceneSettings : MonoBehaviour {
        public static int MaxQueuedFrames;
        public int _MaxQueuedFrames;

        public static int VSyncCount;
        public int _VSyncCount;

        public static bool EnableRuntimeVSyncControl;
        public bool _EnableRuntimeVSyncControl;

        public static Vector2Int StartingResolution;
        public Vector2Int _StartingResolution;

        void Awake() {
            MaxQueuedFrames = this._MaxQueuedFrames;
            VSyncCount = this._VSyncCount;
            EnableRuntimeVSyncControl = this._EnableRuntimeVSyncControl;
            StartingResolution = this._StartingResolution;
        }
    }
}
