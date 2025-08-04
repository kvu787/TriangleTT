using System;
using UnityEngine;

namespace DrivingGameV2 {
    public static class MenuLogic {
        public static bool IsMenuOpened = false;
        public static string LapTimesFilePath;

        public static void Init() {
            LapTimesFilePath = $"{Application.persistentDataPath}/{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff} LapTimes.txt".Replace("/", "\\");

            SceneObjects.LapTimesFilePathInputField.text = LapTimesFilePath;
            SceneObjects.DebugLogFilePathInputField.text = $"{Application.persistentDataPath}/Player.log".Replace("/", "\\");

            SceneObjects.Menu.SetActive(false);
            SceneObjects.OpenMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(true);
                IsMenuOpened = true;
            });
            SceneObjects.CloseMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(false);
                IsMenuOpened = false;
            });
        }
    }
}
