using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DrivingGameV2 {
    public class Checkpointer {
        private readonly Collider car;
        private readonly IList<Collider> checkpoints;
        private readonly Stopwatch lapTimer;
        private List<TimeSpan> cumulativeTimes;

        private int nextCheckpointIndex;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private Collider nextCheckpoint => this.checkpoints[this.nextCheckpointIndex];

        public Checkpointer(Collider car, IList<Collider> checkpoints) {
            this.car = car;
            this.checkpoints = checkpoints;
            this.lapTimer = new Stopwatch();
            this.cumulativeTimes = new List<TimeSpan>();
            this.nextCheckpointIndex = 0;
        }

        private void AdvanceCheckpoint() {
            this.nextCheckpointIndex = (this.nextCheckpointIndex + 1) % this.checkpoints.Count;
        }

        private void OutputLapTimings() {
            for (int i = 0; i < this.checkpoints.Count - 1; i++) {
                Debug.Log($"Section {i + 1} cumulative time: {this.cumulativeTimes[i]}");
            }
            Debug.Log($"Total lap time: {this.cumulativeTimes.Last()}");
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
