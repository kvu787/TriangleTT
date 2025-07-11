using UnityEngine;
using UnityEngine.InputSystem;

namespace DrivingGameV2
{
    public class GameManager : MonoBehaviour
    {
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
        void Start()
        {
            this.playerControls = new PlayerControls();
            this.playerControls.Enable();
            this.carState =
                new CarState
                {
                    position = this.CarObject.transform.position,
                    rotation = this.CarObject.transform.rotation,
                    velocity = Vector3.zero,
                };
            this.initialCarState = this.carState;
            this.camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            bool isTimerDone = AdvanceResetTimer();
            if (isTimerDone)
            {
                this.UpdateCarState(Time.deltaTime);
            }

            if (this.playerControls.Player.CarReset.WasPressedThisFrame())
            {
                this.ResetCar();
            }

            this.WriteCarStateToCarObject();
        }

        private bool AdvanceResetTimer()
        {
            if (this.isResetTimerRunning)
            {
                this.resetTimerRemainingTime -= Time.deltaTime;
                if (this.resetTimerRemainingTime <= 0)
                {
                    this.isResetTimerRunning = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void UpdateCarState(float deltaTime)
        {
            float carBrakeInput = playerControls.Player.CarBrake.ReadValue<float>();
            if (carBrakeInput != 0)
            {
                if (this.carState.velocity != Vector3.zero)
                {
                    var opposingVec = (-1 * this.carState.velocity).normalized;
                    var velocityDelta = opposingVec * carBrakeInput * carMaxBrake * deltaTime;
                    if (velocityDelta.magnitude >= this.carState.velocity.magnitude)
                    {
                        this.carState.velocity = Vector3.zero;
                    }
                    else
                    {
                        this.carState.velocity += velocityDelta;
                    }
                }
            }
            else
            {
                var carAccelInput = playerControls.Player.CarAccel.ReadValue<Vector2>();
                if (carAccelInput.magnitude > 0)
                {
                    Vector3 velocityDelta = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0)
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

        private void ResetCar()
        {
            this.isResetTimerRunning = true;
            this.resetTimerRemainingTime = ResetTimerDuration;
            this.carState = initialCarState;
        }

        private void WriteCarStateToCarObject()
        {
            this.CarObject.transform.position = this.carState.position;

            if (this.carState.velocity != Vector3.zero)
            {
                // rotate the car to match the velocity direction
                this.carState.rotation = Quaternion.LookRotation(this.carState.velocity);
            }
            CarObject.transform.eulerAngles = this.carState.rotation.eulerAngles;
        }
    }

    struct CarState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
    }
}
