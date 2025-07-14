using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace DrivingGameV2 {
    public enum Tags {
        Barrier,
    }

    public class GameManager : MonoBehaviour {
        public GameObject CarObject;
        public Collider CarCollider;

        private CarState carState;
        private Camera mainCamera;
        private PlayerControls playerControls;

        private const float carMaxAccel = 100; // meters per sec per sec
        private const float carMaxBrake = 200; // meters per sec per sec
        private const float carMaxSpeed = 100; // meters per sec

        private CarState initialCarState;

        private bool isResetTimerRunning = false;
        private float resetTimerRemainingTime = 0;
        private const float ResetTimerDuration = 0.6f;

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
        }

        // Update is called once per frame
        void Update() {
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

        private bool HasCarCollidedWithBarrier() {
            List<Collider> barrierColliders = GameObject.FindGameObjectsWithTag(Tags.Barrier.ToString()).Select(x => x.GetComponent<Collider>()).ToList();
            Assert.IsFalse(barrierColliders.Any(x => x is null));
            foreach (Collider barrierCollider in barrierColliders) {
                // Here I use Physics.ComputePenetration because I was having issues with the more commonly used Collider.OnTriggerEnter.
                // When the car collided with a barrier right next to its reset position and the reset timeout was too low, OnTriggerEnter
                // would fail to trigger because the collisions happened too frequently (within 10 ms or less) and collision checking was tied
                // to the 50 Hz FixedUpdate. This meant OnTriggerEnter wouldn't always trigger. Increasing the FixedUpdate frequency resolved this,
                // but I think it's more straightforward to do inline collision checking with ComputePenetration.
                bool isOverlapping =
                    Physics.ComputePenetration(
                        this.CarCollider, this.CarCollider.transform.position, this.CarCollider.transform.rotation,
                        barrierCollider, barrierCollider.transform.position, barrierCollider.transform.rotation,
                        out _, out _);
                if (isOverlapping) {
                    return true;
                }
            }

            return false;
        }

        private bool CarInputTimeout() {
            if (this.isResetTimerRunning) {
                this.resetTimerRemainingTime -= Time.deltaTime;
                if (this.resetTimerRemainingTime <= 0) {
                    this.isResetTimerRunning = false;
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
            if (carBrakeInput != 0) {
                if (this.carState.velocity != Vector3.zero) {
                    Vector3 opposingVec = (-1 * this.carState.velocity).normalized;
                    Vector3 velocityDelta = opposingVec * carBrakeInput * carMaxBrake * deltaTime;
                    if (velocityDelta.magnitude >= this.carState.velocity.magnitude) {
                        this.carState.velocity = Vector3.zero;
                    } else {
                        this.carState.velocity += velocityDelta;
                    }
                }
            } else {
                Vector2 carAccelInput = this.playerControls.Player.CarAccel.ReadValue<Vector2>();
                if (carAccelInput.magnitude > 0) {
                    Vector3 velocityDelta = Quaternion.Euler(0, this.mainCamera.transform.eulerAngles.y, 0)
                        * new Vector3(x: carAccelInput.x, y: 0, z: carAccelInput.y)
                        * carMaxAccel
                        * deltaTime;
                    velocityDelta.y = 0;
                    this.carState.velocity += velocityDelta;
                }
            }

            // update car state
            this.carState.velocity = Vector3.ClampMagnitude(this.carState.velocity, carMaxSpeed);
            this.carState.position += this.carState.velocity * deltaTime;
        }

        private void ResetCar() {
            this.isResetTimerRunning = true;
            this.resetTimerRemainingTime = ResetTimerDuration;
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
