using UnityEngine;

namespace DrivingGameV2 {
    public class GameLoop : MonoBehaviour {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {
            Input.Init();
            VSyncLogic.Init();
            MaxQueuedFramesLogic.Init();
            CarSwitchLogic.InitCars();
            CarLogic.Init();
            CarResetLogic.Init();
        }

        // Update is called once per frame
        void Update() {
            VSyncLogic.UpdateVSyncSetting();
            CheckpointLogic.UpdateLapTimes();
            CarResetLogic.UpdateTimeout();

            if (CarSwitchLogic.ProcessCarSwitch() || Input.ResetCarEvent || CollisionLogic.HasCarCollidedWithBarrier()) {
                CarResetLogic.ResetCar();
                CarLogic.WriteCarStateToCarObject();
                return;
            }

            if (CarResetLogic.IsTimedOut) {
                return;
            }

            CarLogic.ProcessCarInputAndPhysics();
            CarLogic.WriteCarStateToCarObject();
        }
    }
}
