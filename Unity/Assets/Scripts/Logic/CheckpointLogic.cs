using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace DrivingGameV2 {
    public static class CheckpointLogic {
        private static readonly List<Collider> checkpoints = new() {
            SceneObjects.FinishLineCollider,
            SceneObjects.CheckpointCollider1,
            SceneObjects.CheckpointCollider2,
            SceneObjects.CheckpointCollider3,
        };

        private static readonly Stopwatch lapTimer = new();
        private static List<TimeSpan> cumulativeTimes = new();
        private static int lapsCompleted = 0;

        private static int nextCheckpointIndex = 0;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private static Collider nextCheckpoint => checkpoints[nextCheckpointIndex];

        private static void AdvanceCheckpoint() {
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpoints.Count;
        }

        private static void OutputLapTimings() {
            using StreamWriter writer = File.AppendText(MenuLogic.LapTimesFilePath);
            writer.WriteLine($"Lap {lapsCompleted}");
            writer.WriteLine($"Total lap time: {cumulativeTimes.Last()}");
            TimeSpan prevCumulativeTime = TimeSpan.Zero;
            for (int i = 0; i < checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} time: {cumulativeTimes[i] - prevCumulativeTime}");
                prevCumulativeTime = cumulativeTimes[i];
            }
            for (int i = 0; i < checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} cumulative time: {cumulativeTimes[i]}");
            }
            writer.WriteLine();
        }

        public static void Reset() {
            lapTimer.Reset();
            cumulativeTimes = new List<TimeSpan>();
            nextCheckpointIndex = 0;
        }

        public static void UpdateLapTimes() {
            if (!CollisionLogic.HasCollided(CarSwitchLogic.CurrentCar.Collider, nextCheckpoint)) {
                return;
            }

            if (nextCheckpointIndex == 0) {
                if (cumulativeTimes.Count == 0) {
                    lapTimer.Start();
                } else {
                    lapsCompleted++;
                    cumulativeTimes.Add(lapTimer.Elapsed);
                    OutputLapTimings();
                    cumulativeTimes = new List<TimeSpan>();
                    lapTimer.Restart();
                }
            } else {
                cumulativeTimes.Add(lapTimer.Elapsed);
            }
            AdvanceCheckpoint();
        }
    }
}
