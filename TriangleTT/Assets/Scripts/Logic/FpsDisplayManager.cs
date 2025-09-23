using System;
using System.Diagnostics;
using UnityEngine;

namespace TriangleTT {
    public static class FpsDisplayManager {
        private static Stopwatch Stopwatch;

        public static void Update() {
            if (Stopwatch is not null) {
                if (Stopwatch.Elapsed < TimeSpan.FromMilliseconds(100)) {
                    return;
                } else {
                    Stopwatch.Restart();
                }
            }

            SceneObjects.FpsTextLabel.text = $"FPS: {1f / Time.deltaTime}";
        }

        public static void SetSlowFpsCounter(bool enable) {
            if (enable) {
                Stopwatch = new Stopwatch();
                Stopwatch.Start();
            } else {
                Stopwatch = null;
            }
        }
    }
}
