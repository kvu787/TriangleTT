using System;
using UnityEngine;

namespace DrivingGameV2 {
    public static class MenuLogic {
        public static bool IsMenuOpened = false;
        public static string LapTimesFilePath;

        public static void Init() {
            SceneObjects.Menu.SetActive(false);

            LapTimesFilePath = $"{Application.persistentDataPath}/{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff} LapTimes.txt".Replace("/", "\\");
            SceneObjects.LapTimesFilePathInputField.text = LapTimesFilePath;
            SceneObjects.DebugLogFilePathInputField.text = $"{Application.persistentDataPath}/Player.log".Replace("/", "\\");

            SceneObjects.OpenMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(true);
                IsMenuOpened = true;
            });
            SceneObjects.CloseMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(false);
                IsMenuOpened = false;
            });

            SetShadowMode(SceneObjects.ShadowToggle.isOn);
            SceneObjects.ShadowToggle.onValueChanged.AddListener((enable) => SetShadowMode(enable));
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
