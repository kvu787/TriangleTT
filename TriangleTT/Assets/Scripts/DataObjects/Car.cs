using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TriangleTT {
    [Serializable]
    public class Car {
        [SerializeField]
        private string gameObjectName;

        public Dynamic Dynamic;

        [NonSerialized]
        public GameObject GameObject;

        [NonSerialized]
        public Collider Collider;

        public void InitAfterCreateFromJson() {
            Assert.IsTrue(!string.IsNullOrEmpty(this.gameObjectName));
            GameObject gameObject = GameObject.Find(this.gameObjectName);
            Assert.IsNotNull(gameObject);

            // The input gameObject is supposed to be a fixed decoration.
            // The actual gameObject that drives around is a copy.
            this.GameObject = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            this.GameObject.transform.SetFrom(SceneObjects.PlaceholderCarObject.transform);
            this.GameObject.SetActive(false);

            this.Collider = this.GameObject.GetComponentInChildren<Collider>();
            Assert.IsNotNull(this.Collider);
        }
    }
}
