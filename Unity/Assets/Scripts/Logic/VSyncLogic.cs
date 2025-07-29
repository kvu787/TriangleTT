using UnityEngine;
using UnityEngine.InputSystem;

namespace DrivingGameV2 {
    public static class VSyncLogic {
        public static void Init() {
            QualitySettings.vSyncCount = SceneSettings.VSyncCount;
        }

        public static void UpdateVSyncSetting() {
            if (SceneSettings.EnableRuntimeVSyncControl) {
                if (Keyboard.current.digit0Key.isPressed) {
                    QualitySettings.vSyncCount = 0;
                }
                if (Keyboard.current.digit1Key.isPressed) {
                    QualitySettings.vSyncCount = 1;
                }
                if (Keyboard.current.digit2Key.isPressed) {
                    QualitySettings.vSyncCount = 2;
                }
                if (Keyboard.current.digit3Key.isPressed) {
                    QualitySettings.vSyncCount = 3;
                }
                if (Keyboard.current.digit4Key.isPressed) {
                    QualitySettings.vSyncCount = 4;
                }
            }
        }
    }
}
