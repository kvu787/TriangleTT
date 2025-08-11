using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TriangleTT {
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
        public static Light Sunlight;
        public Light _Sunlight;

        public static TMP_Text FpsTextLabel;
        public TMP_Text _FpsTextLabel;
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
        public static Toggle ShadowToggle;
        public Toggle _ShadowToggle;
        public static TMP_Dropdown VSyncDropdown;
        public TMP_Dropdown _VSyncDropdown;
        public static Slider RenderScaleSlider;
        public Slider _RenderScaleSlider;

        public void Init() {
            UnityNullExtensions.AssignIfNull(ref FinishLineCollider, this._FinishLineCollider);
            UnityNullExtensions.AssignIfNull(ref CheckpointCollider1, this._CheckpointCollider1);
            UnityNullExtensions.AssignIfNull(ref CheckpointCollider2, this._CheckpointCollider2);
            UnityNullExtensions.AssignIfNull(ref CheckpointCollider3, this._CheckpointCollider3);
            UnityNullExtensions.AssignIfNull(ref PlaceholderCarObject, this._PlaceholderCarObject);
            UnityNullExtensions.AssignIfNull(ref CameraAnglePivot, this._CameraAnglePivot);
            UnityNullExtensions.AssignIfNull(ref Sunlight, this._Sunlight);

            UnityNullExtensions.AssignIfNull(ref FpsTextLabel, this._FpsTextLabel);
            UnityNullExtensions.AssignIfNull(ref OpenMenuButton, this._OpenMenuButton);
            UnityNullExtensions.AssignIfNull(ref Menu, this._Menu);
            UnityNullExtensions.AssignIfNull(ref CloseMenuButton, this._CloseMenuButton);
            UnityNullExtensions.AssignIfNull(ref LapTimesFilePathInputField, this._LapTimesFilePathInputField);
            UnityNullExtensions.AssignIfNull(ref DebugLogFilePathInputField, this._DebugLogFilePathInputField);
            UnityNullExtensions.AssignIfNull(ref ShadowToggle, this._ShadowToggle);
            UnityNullExtensions.AssignIfNull(ref VSyncDropdown, this._VSyncDropdown);
            UnityNullExtensions.AssignIfNull(ref RenderScaleSlider, this._RenderScaleSlider);
        }
    }
}
