using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DrivingGameV2
{
    public class GameManager : MonoBehaviour
    {
        public GameObject CarObject;
        public Collider CarCollider;
        public Collider FinishLineCollider;

        private CarState carState;
        private new Camera camera;
        private PlayerControls playerControls;

        private const float carMaxAccel = 100; // meters per sec per sec
        private const float carMaxBrake = 200; // meters per sec per sec
        private const float carMaxSpeed = 1000; // meters per sec

        private CarState initialCarState;

        //////////////// TOGGLE INLINE IN THE INSPECTOR ///////////
        public bool isInlineCheckEnabled;
        ///////////////////////////////////////////////////////////

        public bool isOverlapping = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.playerControls = new PlayerControls();
            this.playerControls.Enable();
            this.carState = new CarState { rotation = this.CarObject.transform.rotation, position = this.CarObject.transform.position, velocity = Vector3.zero };

            //////////////// ADJUST HARDCODED VELOCITY HERE /////////////////

            // 12 meters / frame
            //this.carState.velocity = new Vector3 { x = -706 };

            // 10 meters / frame
            //this.carState.velocity = new Vector3 { x = -588 };

            // 7.5 meters / frame
            this.carState.velocity = new Vector3 { x = -441 };

            // 5 meters / frame
            //this.carState.velocity = new Vector3 { x = -294 };

            // 2 meters / frame
            //this.carState.velocity = new Vector3 { x = -117.6f };

            // 1 meter / frame
            //this.carState.velocity = new Vector3 { x = -58.8f };

            // 0.1 meters / frame
            //this.carState.velocity = new Vector3 { x = -5.88f };

            /////////////////////////////////////////////////////////////////

            this.initialCarState = this.carState;
            this.camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"{Time.frameCount}");

            this.UpdateCarState(1f / 60f);
            this.WriteCarStateToCarObject();

            bool isOverlappingInline = false;
            if (this.isInlineCheckEnabled)
            {
                isOverlappingInline = 
                    Physics.ComputePenetration(
                        CarCollider, CarCollider.transform.position, CarCollider.transform.rotation,
                        FinishLineCollider, FinishLineCollider.transform.position, FinishLineCollider.transform.rotation,
                        out _, out _);
                if (isOverlappingInline)
                {
                    Debug.Log($"{Time.frameCount} INLINE COLLISION");
                }
            }

            this.isOverlapping = this.isOverlapping || isOverlappingInline;

            if (this.isOverlapping)
            {
                this.ResetCar();
                this.WriteCarStateToCarObject();
                this.isOverlapping = false;
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
                    Debug.Log("ACCEL");
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
            Debug.Log($"{Time.frameCount} RESET");
            this.carState = this.initialCarState;
        }

        private void WriteCarStateToCarObject()
        {
            this.CarObject.transform.position = this.carState.position;

            Quaternion newCarRotation;
            if (this.carState.velocity == Vector3.zero)
            {
                newCarRotation = this.carState.rotation;
            }
            else
            {
                // rotate the car to match the velocity direction
                newCarRotation = Quaternion.LookRotation(this.carState.velocity);
            }
            this.carState.rotation = newCarRotation;
            CarObject.transform.eulerAngles = newCarRotation.eulerAngles;
        }
    }

    struct CarState
    {
        public Quaternion rotation;
        public Vector3 position;
        public Vector3 velocity;
    }
}
