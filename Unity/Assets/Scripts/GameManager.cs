using UnityEngine;
using UnityEngine.InputSystem;

namespace DrivingGameV1
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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.playerControls = new PlayerControls();
            this.playerControls.Enable();
            this.carState = new CarState { positionMeters = this.CarObject.transform.position, velocity = Vector3.zero };
            this.camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;

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

            // update car state and object
            this.carState.velocity = Vector3.ClampMagnitude(this.carState.velocity, carMaxSpeed);
            this.carState.positionMeters += this.carState.velocity * deltaTime;
            CarObject.transform.position = this.carState.positionMeters;

            // rotate the car to match the velocity direction
            if (this.carState.velocity != Vector3.zero)
            {
                CarObject.transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(this.carState.velocity).eulerAngles.y, 0);
            }
        }
    }


    class CarState
    {
        public Vector3 positionMeters;
        public Vector3 velocity;
    }
}
