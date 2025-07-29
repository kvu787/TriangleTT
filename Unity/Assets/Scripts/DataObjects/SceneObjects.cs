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

        public static GameObject GreenCarObject;
        public GameObject _GreenCarObject;
        public static GameObject BlueCarObject;
        public GameObject _BlueCarObject;
        public static GameObject RedCarObject;
        public GameObject _RedCarObject;
        public static GameObject YellowCarObject;
        public GameObject _YellowCarObject;
        public static GameObject CyanCarObject;
        public GameObject _CyanCarObject;
        public static GameObject MagentaCarObject;
        public GameObject _MagentaCarObject;

        public static Transform CameraAnglePivot;
        public Transform _CameraAnglePivot;

        private void Awake() {
            FinishLineCollider = this._FinishLineCollider;
            CheckpointCollider1 = this._CheckpointCollider1;
            CheckpointCollider2 = this._CheckpointCollider2;
            CheckpointCollider3 = this._CheckpointCollider3;

            PlaceholderCarObject = this._PlaceholderCarObject;

            GreenCarObject = this._GreenCarObject;
            BlueCarObject = this._BlueCarObject;
            RedCarObject = this._RedCarObject;
            YellowCarObject = this._YellowCarObject;
            CyanCarObject = this._CyanCarObject;
            MagentaCarObject = this._MagentaCarObject;

            CameraAnglePivot = this._CameraAnglePivot;
        }
    }
}
