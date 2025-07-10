using UnityEngine;

namespace DrivingGameV2
{
    public class CollisionHandler : MonoBehaviour
    {
        public GameManager GameManager;

        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{Time.frameCount} TRIGGER: Car collided with name={other.name}, tag={other.tag}");
            this.GameManager.isOverlapping = true;
        }
    }
}
