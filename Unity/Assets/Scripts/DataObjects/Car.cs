using UnityEngine;
using UnityEngine.Assertions;

namespace DrivingGameV2 {
    public class Car {
        public readonly GameObject GameObject;
        public readonly Collider Collider;
        public readonly Dynamic Dynamic;

        public Car(GameObject gameObject, Dynamic dynamic) {
            Assert.IsNotNull(gameObject);

            // The input gameObject is supposed to be a fixed decoration.
            // The actual gameObject that drives around is a copy.
            this.GameObject = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            this.GameObject.transform.SetFrom(SceneObjects.PlaceholderCarObject.transform);
            this.GameObject.SetActive(false);

            this.Collider = this.GameObject.GetComponentInChildren<Collider>();
            Assert.IsNotNull(this.Collider);

            this.Dynamic = dynamic;
        }
    }
}
