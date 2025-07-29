using System.Collections.Generic;

namespace DrivingGameV2 {
    public static class CarSwitchLogic {
        public static Car CurrentCar => Cars[CarIndex];

        // The init value of this determines the starting car.
        // I want the blue car to be default, so I set it to 1 here because I assume
        // that the blue car is at index=1.
        private static int CarIndex = 1;

        private static List<Car> Cars;

        public static void InitCars() {
            SceneObjects.PlaceholderCarObject.SetActive(false);

            Cars = new List<Car> {
                new(
                    gameObject: SceneObjects.GreenCarObject,
                    dynamic: new(
                        velocityLimiter: 75,
                        accelerationMap: new(
                            forward: 100,
                            reverse: 200,
                            left: 100,
                            right: 100))),
                new(
                    gameObject: SceneObjects.BlueCarObject,
                    dynamic: new(
                        velocityLimiter: 150,
                        accelerationMap: new(
                            forward: 100,
                            reverse: 200,
                            left: 100,
                            right: 100))),
                new(
                    gameObject: SceneObjects.RedCarObject,
                    dynamic: new(
                        velocityLimiter: -1,
                        accelerationMap: new(
                            forward: 300,
                            reverse: 450,
                            left: 150,
                            right: 150))),
                new(
                    gameObject: SceneObjects.YellowCarObject,
                    dynamic: new(
                        velocityLimiter: -1,
                        accelerationMap: new(
                            forward: 500,
                            reverse: 1000,
                            left: 500,
                            right: 500))),
                new(
                    gameObject: SceneObjects.CyanCarObject,
                    dynamic: new(
                        velocityLimiter: -1,
                        accelerationMap: new(
                            forward: 100,
                            reverse: 200,
                            left: 400,
                            right: 400))),
                new(
                    gameObject: SceneObjects.MagentaCarObject,
                    dynamic: new(
                        velocityLimiter: -1,
                        accelerationMap: new(
                            forward: 1000,
                            reverse: 2000,
                            left: 100,
                            right: 100))),
            };

            CurrentCar.GameObject.SetActive(true);
        }

        public static bool ProcessCarSwitch() {
            if (!Input.NextCarEvent && !Input.PrevCarEvent) {
                return false;
            }
            CurrentCar.GameObject.SetActive(false);
            if (Input.NextCarEvent) {
                CarIndex += 1;
                CarIndex %= Cars.Count;
            } else if (Input.PrevCarEvent) {
                if (CarIndex == 0) {
                    CarIndex = Cars.Count - 1;
                } else {
                    CarIndex -= 1;
                }
            }
            CurrentCar.GameObject.SetActive(true);
            return true;
        }
    }
}
