using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TriangleTT {
    public static class CollisionLogic {
        private static IEnumerable<Collider> Colliders;

        public static void Init() {
            List<GameObject> gameObjects = new();
            gameObjects.AddRange(GameObject.FindGameObjectsWithTag(Tags.Barrier.ToString()));
            gameObjects.AddRange(GameObject.FindGameObjectsWithTag(Tags.Cone.ToString()));
            Colliders = gameObjects.Select(x => x.GetComponent<Collider>());
            Assert.IsFalse(Colliders.Any(x => x is null));
        }

        public static bool HasCollided(Collider a, Collider b) {
            // I use Physics.ComputePenetration because I was having issues with the more commonly used Collider.OnTriggerEnter.
            // When the car collided with a barrier right next to its reset position and the reset timeout was too low, OnTriggerEnter
            // would fail to trigger because the collisions happened too frequently (within 10 ms or less) and collision checking was tied
            // to the 50 Hz FixedUpdate. This meant OnTriggerEnter wouldn't always trigger. Increasing the FixedUpdate frequency resolved this,
            // but I think it's more straightforward to do inline collision checking with ComputePenetration.
            return Physics.ComputePenetration(
                a, a.transform.position, a.transform.rotation,
                b, b.transform.position, b.transform.rotation,
                out _, out _);
        }

        public static bool HasCarCollided() {
            foreach (Collider collider in Colliders) {
                if (HasCollided(CarSwitcher.CurrentCar.Collider, collider)) {
                    return true;
                }
            }
            return false;
        }
    }
}
