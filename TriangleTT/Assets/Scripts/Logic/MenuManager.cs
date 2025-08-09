using UnityEngine;

namespace TriangleTT {
    public static class MenuManager {
        public static void Init() {
            SceneObjects.Menu.SetActive(false);

            SceneObjects.LapTimesFilePathInputField.text = Checkpointer.LapTimesFilePath;
            SceneObjects.DebugLogFilePathInputField.text = $"{Application.persistentDataPath}/Player.log".Replace("/", "\\");

            SceneObjects.OpenMenuButton.onClick.AddListener(() => SceneObjects.Menu.SetActive(true));
            SceneObjects.CloseMenuButton.onClick.AddListener(() => SceneObjects.Menu.SetActive(false));

            SetShadowMode(SceneObjects.ShadowToggle.isOn);
            SceneObjects.ShadowToggle.onValueChanged.AddListener((enable) => SetShadowMode(enable));

            QualitySettings.vSyncCount = SceneObjects.VSyncDropdown.value;
            SceneObjects.VSyncDropdown.onValueChanged.AddListener((index) => QualitySettings.vSyncCount = index);
        }

        private static void SetShadowMode(bool isEnabled) {
            if (isEnabled) {
                SceneObjects.Sunlight.shadows = LightShadows.Hard;
            } else {
                SceneObjects.Sunlight.shadows = LightShadows.None;
            }
        }
    }
}
