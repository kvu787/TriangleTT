using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace DrivingGameV2 {
    public enum Tags {
        Barrier,
    }

    public class GameManager : MonoBehaviour {
        public GameObject CarObject;
        public Collider CarCollider;
        public int MaxQueuedFrames;
        public bool EnableVSyncControl;
        public Collider FinishLineCollider;
        public Collider CheckpointCollider1;
        public Collider CheckpointCollider2;
        public Collider CheckpointCollider3;

        private CarState carState;
        private Camera mainCamera;
        private PlayerControls playerControls;

        private const float carMaxAccel = 100; // meters per sec per sec
        private const float carMaxBrake = 200; // meters per sec per sec
        private const float carMaxSpeed = 100; // meters per sec

        private CarState initialCarState;

        private readonly TimeSpan ResetTimerDuration = TimeSpan.FromSeconds(0.6f);
        private Stopwatch resetCarStopwatch;

        private Checkpointer checkpointer;

        void Awake() {
            QualitySettings.maxQueuedFrames = this.MaxQueuedFrames;
            Debug.Log($"QualitySettings.maxQueuedFrames = {QualitySettings.maxQueuedFrames}");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {
            this.playerControls = new PlayerControls();
            this.playerControls.Enable();
            this.carState =
                new CarState {
                    position = this.CarObject.transform.position,
                    rotation = this.CarObject.transform.rotation,
                    velocity = Vector3.zero,
                };
            this.initialCarState = this.carState;
            this.mainCamera = Camera.main;
            this.resetCarStopwatch = new Stopwatch();
            this.checkpointer = new Checkpointer(this.CarCollider, new List<Collider>() { this.FinishLineCollider, this.CheckpointCollider1, this.CheckpointCollider2, this.CheckpointCollider3 });
        }

        // Update is called once per frame
        void Update() {
            if (this.EnableVSyncControl) {
                this.HandleVSyncControl();
            }

            this.checkpointer.Update();

            if (this.playerControls.Player.CarReset.WasPressedThisFrame()) {
                this.ResetCar();
            } else if (this.HasCarCollidedWithBarrier()) {
                this.ResetCar();
            } else if (this.CarInputTimeout()) {
                return;
            } else {
                this.ProcessCarInputAndPhysics(Time.deltaTime);
            }

            this.WriteCarStateToCarObject();
        }

        private void HandleVSyncControl() {
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

        public static bool HasCollided(Collider a, Collider b) {
            // I use Physics.ComputePenetration because I was having issues with the more commonly used Collider.OnTriggerEnter.
            // When the car collided with a barrier right next to its reset position and the reset timeout was too low, OnTriggerEnter
            // would fail to trigger because the collisions happened too frequently (within 10 ms or less) and collision checking was tied
            // to the 50 Hz FixedUpdate. This meant OnTriggerEnter wouldn't always trigger. Increasing the FixedUpdate frequency resolved this,
            // but I think it's more straightforward to do inline collision checking with ComputePenetration.
            return Physics.ComputePenetration(
                a, a.transform.position, a.transform.rotation,
                b, b.transform.position, b.transform.rotation,
                out _, out _);
        }

        private bool HasCarCollidedWithBarrier() {
            List<Collider> barrierColliders = GameObject.FindGameObjectsWithTag(Tags.Barrier.ToString()).Select(x => x.GetComponent<Collider>()).ToList();
            Assert.IsFalse(barrierColliders.Any(x => x is null));
            foreach (Collider barrierCollider in barrierColliders) {
                if (HasCollided(this.CarCollider, barrierCollider)) {
                    return true;
                }
            }
            return false;
        }

        private bool CarInputTimeout() {
            if (this.resetCarStopwatch.IsRunning) {
                if (this.resetCarStopwatch.Elapsed > this.ResetTimerDuration) {
                    this.resetCarStopwatch.Reset();
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        private void ProcessCarInputAndPhysics(float deltaTime) {
            float carBrakeInput = this.playerControls.Player.CarBrake.ReadValue<float>();
            if (carBrakeInput == 0) {
                Vector2 carAccelInput = this.playerControls.Player.CarAccel.ReadValue<Vector2>();
                if (carAccelInput.magnitude > 0) {
                    // Map stick input onto XZ plane
                    Vector3 a = new(x: carAccelInput.x, y: 0, z: carAccelInput.y);
                    // Orient with respect to the car by adjusting for the camera rotation
                    Vector3 b = Quaternion.Euler(0, this.mainCamera.transform.eulerAngles.y, 0) * a;
                    // Get the acceleration by scaling to max accel
                    Vector3 c = carMaxAccel * b;
                    // Convert acceleration to change in velocity
                    Vector3 d = deltaTime * c;
                    // The result is the change in velocity for this frame
                    Vector3 velocityDelta = d;
                    velocityDelta.y = 0;
                    this.carState.velocity += velocityDelta;
                }
            } else if (this.carState.velocity != Vector3.zero) {
                Vector3 opposingVec = (-1 * this.carState.velocity).normalized;
                Vector3 velocityDelta = opposingVec * carBrakeInput * carMaxBrake * deltaTime;
                if (velocityDelta.magnitude >= this.carState.velocity.magnitude) {
                    this.carState.velocity = Vector3.zero;
                } else {
                    this.carState.velocity += velocityDelta;
                }
            }

            // update car state
            this.carState.velocity = Vector3.ClampMagnitude(this.carState.velocity, carMaxSpeed);
            this.carState.position += this.carState.velocity * deltaTime;
        }

        private void ResetCar() {
            this.checkpointer.Reset();
            this.resetCarStopwatch.Reset();
            this.resetCarStopwatch.Start();
            this.carState = this.initialCarState;
        }

        private void WriteCarStateToCarObject() {
            this.CarObject.transform.position = this.carState.position;
            if (this.carState.velocity != Vector3.zero) {
                // rotate the car to match the velocity direction
                this.carState.rotation = Quaternion.LookRotation(this.carState.velocity);
            }
            this.CarObject.transform.eulerAngles = this.carState.rotation.eulerAngles;
        }

        private struct CarState {
            internal Vector3 position;
            internal Quaternion rotation;
            internal Vector3 velocity;
        }
    }
}
