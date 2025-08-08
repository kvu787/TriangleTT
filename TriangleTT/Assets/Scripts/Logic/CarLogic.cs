using UnityEngine;

namespace DrivingGameV2 {
    public class CarLogic {
        public static CarState CarState;

        public static void Init() {
            CarState =
                new CarState {
                    Position = SceneObjects.PlaceholderCarObject.transform.position,
                    Rotation = SceneObjects.PlaceholderCarObject.transform.rotation,
                    Velocity = Vector3.zero,
                };
        }

        public static void ProcessCarInputAndPhysics() {
            Dynamic dynamic = CarSwitchLogic.CurrentCar.Dynamic;
            if (Input.Brake == 0) {
                if (Input.Accel.magnitude > 0) {
                    // Map XY input onto XZ world plane
                    Vector3 a = new(Input.Accel.x, 0, Input.Accel.y);

                    // Adjust for camera rotation
                    Vector3 b = Quaternion.Euler(0, SceneObjects.CameraAnglePivot.eulerAngles.y, 0) * a;

                    // Orient with respect to the car
                    Vector3 c = Quaternion.Inverse(CarState.Rotation) * b;

                    // Apply acceleration map
                    Vector3 d = c;
                    if (d.x > 0) {
                        d.x *= dynamic.AccelerationMap.Right;
                    } else if (d.x < 0) {
                        d.x *= dynamic.AccelerationMap.Left;
                    }
                    if (d.z > 0) {
                        d.z *= dynamic.AccelerationMap.Forward;
                    } else if (d.z < 0) {
                        d.z *= dynamic.AccelerationMap.Reverse;
                    }

                    // Rotate back to world space
                    Vector3 e = CarState.Rotation * d;

                    // Convert acceleration to change in velocity
                    Vector3 f = Time.deltaTime * e;

                    // Just to be safe, zero out the y coordinate
                    f.y = 0;

                    // The result is the change in velocity for this frame
                    Vector3 velocityDelta = f;

                    // Update car velocity
                    CarState.Velocity += velocityDelta;
                }
            } else if (CarState.Velocity != Vector3.zero) {
                Vector3 opposingVec = (-1 * CarState.Velocity).normalized;
                Vector3 velocityDelta = opposingVec * Input.Brake * dynamic.AccelerationMap.Reverse * Time.deltaTime;
                if (velocityDelta.magnitude >= CarState.Velocity.magnitude) {
                    CarState.Velocity = Vector3.zero;
                } else {
                    CarState.Velocity += velocityDelta;
                }
            }

            // update car position
            if (dynamic.VelocityLimiter >= 0) {
                CarState.Velocity = Vector3.ClampMagnitude(CarState.Velocity, dynamic.VelocityLimiter);
            }
            CarState.Position += CarState.Velocity * Time.deltaTime;

            // rotate the car to match the velocity direction
            if (CarState.Velocity != Vector3.zero) {
                CarState.Rotation = Quaternion.LookRotation(CarState.Velocity);
            }
        }

        public static void WriteCarStateToCarObject() {
            CarSwitchLogic.CurrentCar.GameObject.transform.position = CarState.Position;
            CarSwitchLogic.CurrentCar.GameObject.transform.rotation = CarState.Rotation;
        }
    }
}
