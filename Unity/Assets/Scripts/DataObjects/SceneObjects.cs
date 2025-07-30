using UnityEngine;

namespace DrivingGameV2 {
    public class SceneObjects : MonoBehaviour {
        public static Collider FinishLineCollider;
        public Collider _FinishLineCollider;
        public static Collider CheckpointCollider1;
        public Collider _CheckpointCollider1;
        public static Collider CheckpointCollider2;
        public Collider _CheckpointCollider2;
        public static Collider CheckpointCollider3;
        public Collider _CheckpointCollider3;

        public static GameObject PlaceholderCarObject;
        public GameObject _PlaceholderCarObject;

        public static Transform CameraAnglePivot;
        public Transform _CameraAnglePivot;

        private void Awake() {
            FinishLineCollider = this._FinishLineCollider;
            CheckpointCollider1 = this._CheckpointCollider1;
            CheckpointCollider2 = this._CheckpointCollider2;
            CheckpointCollider3 = this._CheckpointCollider3;

            PlaceholderCarObject = this._PlaceholderCarObject;

            CameraAnglePivot = this._CameraAnglePivot;
        }
    }
}
