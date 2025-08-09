using System;
using System.Diagnostics;
using UnityEngine;

namespace TriangleTT {
    public static class AspectRatioEnforcer {
        private static Vector2Int AspectRatioXY = new(16, 9);
        private static readonly float AspectRatio = (float)AspectRatioXY.x / AspectRatioXY.y;
        private static readonly Stopwatch Stopwatch = new();
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(1);

        public static void Update() {
            if (Stopwatch.IsRunning) {
                if (Stopwatch.Elapsed > Timeout) {
                    int multiplier = Screen.width / AspectRatioXY.x;
                    Screen.SetResolution(AspectRatioXY.x * multiplier, AspectRatioXY.y * multiplier, FullScreenMode.Windowed);
                    Stopwatch.Reset();
                }
            } else {
                float currentRatio = (float)Screen.width / Screen.height;
                if (!Mathf.Approximately(currentRatio, AspectRatio)) {
                    Stopwatch.Start();
                }
            }
        }
    }
}
