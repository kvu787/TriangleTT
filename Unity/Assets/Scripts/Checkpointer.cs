using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace DrivingGameV2 {
    public class Checkpointer {
        private readonly Collider car;
        private readonly IList<Collider> checkpoints;
        private readonly Stopwatch lapTimer;
        private List<TimeSpan> cumulativeTimes;
        private int lapsCompleted;

        private int nextCheckpointIndex;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private Collider nextCheckpoint => this.checkpoints[this.nextCheckpointIndex];

        public Checkpointer(Collider car, IList<Collider> checkpoints) {
            this.car = car;
            this.checkpoints = checkpoints;
            this.lapTimer = new Stopwatch();
            this.cumulativeTimes = new List<TimeSpan>();
            this.lapsCompleted = 0;
            this.nextCheckpointIndex = 0;
        }

        private void AdvanceCheckpoint() {
            this.nextCheckpointIndex = (this.nextCheckpointIndex + 1) % this.checkpoints.Count;
        }

        private void OutputLapTimings() {
            string lapTimesFilePath = UI.LapTimesFilePath;
            using StreamWriter writer = File.AppendText(lapTimesFilePath);
            writer.WriteLine($"Lap {this.lapsCompleted}");
            writer.WriteLine($"Total lap time: {this.cumulativeTimes.Last()}");
            TimeSpan prevCumulativeTime = TimeSpan.Zero;
            for (int i = 0; i < this.checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} time: {this.cumulativeTimes[i] - prevCumulativeTime}");
                prevCumulativeTime = this.cumulativeTimes[i];
            }
            for (int i = 0; i < this.checkpoints.Count; i++) {
                writer.WriteLine($"Section {i + 1} cumulative time: {this.cumulativeTimes[i]}");
            }
            writer.WriteLine();
        }

        public void Reset() {
            this.lapTimer.Reset();
            this.cumulativeTimes = new List<TimeSpan>();
            this.nextCheckpointIndex = 0;
        }

        public void Update() {
            if (GameManager.HasCollided(this.car, this.nextCheckpoint)) {
                if (this.nextCheckpointIndex == 0) {
                    if (this.cumulativeTimes.Count == 0) {
                        this.lapTimer.Start();
                    } else {
                        this.lapsCompleted++;
                        this.cumulativeTimes.Add(this.lapTimer.Elapsed);
                        this.OutputLapTimings();
                        this.cumulativeTimes = new List<TimeSpan>();
                        this.lapTimer.Restart();
                    }
                } else {
                    this.cumulativeTimes.Add(this.lapTimer.Elapsed);
                }
                this.AdvanceCheckpoint();
            }
        }
    }
}
