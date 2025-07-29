
using UnityEngine;

namespace DrivingGameV2 {
    public static class Input {
        private static PlayerControls.PlayerActions PlayerActions;

        public static void Init() {
            PlayerControls playerControls = new();
            playerControls.Enable();
            PlayerActions = playerControls.Player;
        }

        public static bool NextCarEvent => PlayerActions.CarSwitchNext.WasPressedThisFrame();
        public static bool PrevCarEvent => PlayerActions.CarSwitchPrev.WasPressedThisFrame();
        public static bool ResetCarEvent => PlayerActions.CarReset.WasPressedThisFrame();
        public static Vector2 Accel => PlayerActions.CarAccel.ReadValue<Vector2>();
        public static float Brake => PlayerActions.CarBrake.ReadValue<float>();
    }
}
