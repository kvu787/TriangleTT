using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace DrivingGameV2 {
    public static class CarSwitchLogic {
        public static Car CurrentCar => Cars[CurrentCarIndex];
        private static int CurrentCarIndex = 1;

        private static List<Car> Cars;

        private const string defaultGarageFileName = "DefaultGarage.json";
        private const string overrideGarageFileName = "OverrideGarage.json";

        public static void Init() {
            SceneObjects.PlaceholderCarObject.SetActive(false);

            string garageFilePath = ValidateAndGetGarageFilePath();
            string garageFileContents = File.ReadAllText(garageFilePath);
            Garage garage = Garage.CreateFromJson(garageFileContents);
            CurrentCarIndex = garage.StartCarIndex;
            Cars = garage.Cars;

            CurrentCar.GameObject.SetActive(true);
        }

        private static string ValidateAndGetGarageFilePath() {
            string defaultGarageFilePath = Path.Combine(Application.streamingAssetsPath.Replace('/', '\\'), defaultGarageFileName);
            Assert.IsTrue(File.Exists(defaultGarageFilePath), $"Default garage does not exist at {defaultGarageFilePath}");

            string overrideGarageFilePath = Path.Combine(Application.persistentDataPath.Replace('/', '\\'), overrideGarageFileName);
            if (File.Exists(overrideGarageFilePath)) {
                Debug.Log($"Reading from override garage at '{overrideGarageFilePath}'");
                return overrideGarageFilePath;
            } else {
                Debug.Log($"Override garage not found at '{overrideGarageFilePath}', so reading from default garage at '{defaultGarageFilePath}'");
                return defaultGarageFilePath;
            }
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
