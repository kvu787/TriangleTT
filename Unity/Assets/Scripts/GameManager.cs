using UnityEngine;

namespace DrivingGameV2 {
    public class GameManager : MonoBehaviour {
        public GameObject CarObject;

        private CarState carState;
        private new Camera camera;
        private PlayerControls playerControls;

        private const float carMaxAccel = 100; // meters per sec per sec
        private const float carMaxBrake = 200; // meters per sec per sec
        private const float carMaxSpeed = 100; // meters per sec

        private CarState initialCarState;

        private bool isResetTimerRunning = false;
        private float resetTimerRemainingTime = 0;
        private const float ResetTimerDuration = 0.5f;

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
            this.camera = Camera.main;
        }

        // Update is called once per frame
        void Update() {
            bool isTimerDone = this.AdvanceResetTimer();
            if (isTimerDone) {
                this.UpdateCarState(Time.deltaTime);
            }


            if (this.playerControls.Player.CarReset.WasPressedThisFrame()) {
                this.ResetCar();
            }

            this.WriteCarStateToCarObject();
        }

        private bool AdvanceResetTimer() {
            if (this.isResetTimerRunning) {
                this.resetTimerRemainingTime -= Time.deltaTime;
                if (this.resetTimerRemainingTime <= 0) {
                    this.isResetTimerRunning = false;
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        private void UpdateCarState(float deltaTime) {
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
                    Vector3 velocityDelta = Quaternion.Euler(0, this.camera.transform.eulerAngles.y, 0)
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
