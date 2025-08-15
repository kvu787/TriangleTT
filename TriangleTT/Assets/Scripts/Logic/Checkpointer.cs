using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace TriangleTT {
    public static class Checkpointer {
        public static string LapTimesFilePath;

        private static readonly List<Collider> Checkpoints = new() {
            SceneObjects.FinishLineCollider,
            SceneObjects.CheckpointCollider1,
            SceneObjects.CheckpointCollider2,
            SceneObjects.CheckpointCollider3,
        };

        private static readonly Stopwatch LapTimer = new();
        private static List<TimeSpan> CumulativeTimes = new();
        private static int LapsCompleted = 0;

        private static int NextCheckpointIndex = 0;
        private static Collider NextCheckpoint => Checkpoints[NextCheckpointIndex];

        public static void Init() {
            DateTimeOffset currentTime = DateTimeOffset.Now;
            LapTimesFilePath = $"{Application.persistentDataPath}/{currentTime:yyyy-MM-dd_HH-mm-ss-fff} LapTimes.txt".Replace("/", "\\");
            using StreamWriter writer = File.AppendText(LapTimesFilePath);
            writer.WriteLine("Lap times");
            writer.WriteLine();
            writer.WriteLine($"Created at {currentTime:yyyy-MM-dd HH:mm:ss}, {currentTime:zzz}, {TimeZoneInfo.Local}");
            writer.WriteLine();
        }

        private static void AdvanceCheckpoint() {
            NextCheckpointIndex = (NextCheckpointIndex + 1) % Checkpoints.Count;
        }

        private static void OutputLapTimings() {
            using StreamWriter writer = File.AppendText(LapTimesFilePath);
            writer.WriteLine($"Lap {LapsCompleted}");
            writer.WriteLine($"Total lap time: {CumulativeTimes.Last()}");
            TimeSpan prevCumulativeTime = TimeSpan.Zero;
            for (int i = 0; i < Checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} time: {CumulativeTimes[i] - prevCumulativeTime}");
                prevCumulativeTime = CumulativeTimes[i];
            }
            for (int i = 0; i < Checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} cumulative time: {CumulativeTimes[i]}");
            }
            writer.WriteLine();
        }

        public static void Reset() {
            LapTimer.Reset();
            CumulativeTimes = new List<TimeSpan>();
            NextCheckpointIndex = 0;
        }

        public static void UpdateLapTimes() {
            if (!CollisionLogic.HasCollided(CarSwitcher.CurrentCar.Collider, NextCheckpoint)) {
                return;
            }

            if (NextCheckpointIndex == 0) {
                if (CumulativeTimes.Count == 0) {
                    LapTimer.Start();
                } else {
                    LapsCompleted++;
                    CumulativeTimes.Add(LapTimer.Elapsed);
                    OutputLapTimings();
                    CumulativeTimes = new List<TimeSpan>();
                    LapTimer.Restart();
                }
            } else {
                CumulativeTimes.Add(LapTimer.Elapsed);
            }
            AdvanceCheckpoint();
        }
    }
}
