using System;
using System.Diagnostics;

namespace TriangleTT {
    public static class CarResetter {
        public static bool IsTimedOut = false;
        public static CarState InitialCarState;

        private static readonly TimeSpan ResetTimerDuration = TimeSpan.FromSeconds(0.5f);
        private static readonly Stopwatch ResetCarStopwatch = new();

        public static void Init() {
            InitialCarState = CarLogic.CarState;
        }

        public static void UpdateTimeout() {
            if (ResetCarStopwatch.IsRunning) {
                if (ResetCarStopwatch.Elapsed > ResetTimerDuration) {
                    ResetCarStopwatch.Reset();
                    IsTimedOut = false;
                } else {
                    IsTimedOut = true;
                }
            } else {
                IsTimedOut = false;
            }
        }

        public static void ResetCar() {
            ResetCarStopwatch.Restart();
            LapTimer.Reset();
            CarLogic.CarState = InitialCarState;
        }
    }
}
