using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public static Button OpenMenuButton;
        public Button _OpenMenuButton;
        public static GameObject Menu;
        public GameObject _Menu;
        public static Button CloseMenuButton;
        public Button _CloseMenuButton;
        public static TMP_InputField LapTimesFilePathInputField;
        public TMP_InputField _LapTimesFilePathInputField;
        public static TMP_InputField DebugLogFilePathInputField;
        public TMP_InputField _DebugLogFilePathInputField;

        void Awake() {
            FinishLineCollider = this._FinishLineCollider;
            CheckpointCollider1 = this._CheckpointCollider1;
            CheckpointCollider2 = this._CheckpointCollider2;
            CheckpointCollider3 = this._CheckpointCollider3;

            PlaceholderCarObject = this._PlaceholderCarObject;

            CameraAnglePivot = this._CameraAnglePivot;

            OpenMenuButton = this._OpenMenuButton;
            Menu = this._Menu;
            CloseMenuButton = this._CloseMenuButton;
            LapTimesFilePathInputField = this._LapTimesFilePathInputField;
            DebugLogFilePathInputField = this._DebugLogFilePathInputField;
        }
    }
}
