using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace TriangleTT {
    public static class CarSwitcher {
        public static Car CurrentCar => Cars[CurrentCarIndex];
        private static int CurrentCarIndex = 1;

        private static List<Car> Cars;

        private const string GarageFileName = "Garage.json";

        public static void Init() {
            SceneObjects.PlaceholderCarObject.SetActive(false);
            string filePath = Path.Combine(Application.streamingAssetsPath.Replace('/', '\\'), GarageFileName);
            Assert.IsTrue(File.Exists(filePath), $"Garage does not exist at {filePath}");
            string fileContents = File.ReadAllText(filePath);
            Garage garage = Garage.CreateFromJson(fileContents);
            CurrentCarIndex = garage.StartCarIndex;
            Cars = garage.Cars;
            CurrentCar.GameObject.SetActive(true);
        }

        public static bool ProcessCarSwitch() {
            if (!Input.NextCarEvent && !Input.PrevCarEvent) {
                return false;
            }
            CurrentCar.GameObject.SetActive(false);
            if (Input.NextCarEvent) {
                CurrentCarIndex += 1;
                CurrentCarIndex %= Cars.Count;
            } else if (Input.PrevCarEvent) {
                if (CurrentCarIndex == 0) {
                    CurrentCarIndex = Cars.Count - 1;
                } else {
                    CurrentCarIndex -= 1;
                }
            }
            CurrentCar.GameObject.SetActive(true);
            return true;
        }
    }
}
