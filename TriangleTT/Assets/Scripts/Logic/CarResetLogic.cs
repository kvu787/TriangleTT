using System;
using System.Diagnostics;

namespace DrivingGameV2 {
    public static class CarResetLogic {
        public static bool IsTimedOut = false;
        public static CarState InitialCarState;

        private static readonly TimeSpan ResetTimerDuration = TimeSpan.FromSeconds(0.6f);
        private static readonly Stopwatch resetCarStopwatch = new();

        public static void Init() {
            InitialCarState = CarLogic.CarState;
        }

        public static void UpdateTimeout() {
            if (resetCarStopwatch.IsRunning) {
                if (resetCarStopwatch.Elapsed > ResetTimerDuration) {
                    resetCarStopwatch.Reset();
                    IsTimedOut = false;
                } else {
                    IsTimedOut = true;
                }
            } else {
                IsTimedOut = false;
            }
        }

        public static void ResetCar() {
            resetCarStopwatch.Restart();
            CheckpointLogic.Reset();
            CarLogic.CarState = InitialCarState;
        }
    }
}
