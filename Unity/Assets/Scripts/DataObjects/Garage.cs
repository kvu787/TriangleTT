using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingGameV2 {
    [Serializable]
    public class Garage {
        public int StartCarIndex;
        public List<Car> Cars;

        public static Garage CreateFromJson(string jsonString) {
            Garage garage = JsonUtility.FromJson<Garage>(jsonString);
            foreach (Car car in garage.Cars) {
                car.InitAfterCreateFromJson();
            }
            return garage;
        }
    }
}
