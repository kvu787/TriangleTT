using System;
using UnityEngine;

namespace DrivingGameV2 {
    public class GameLoop : MonoBehaviour {
        public static string LapTimesFilePath;

        void Awake() {
            string prefix = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            LapTimesFilePath = $"{Application.persistentDataPath}/{prefix} LapTimes.txt".Replace("/", "\\");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {
            Input.Init();
            VSyncLogic.Init();
            MaxQueuedFramesLogic.Init();
            CarSwitchLogic.InitCars();
            CarLogic.Init();
            CarResetLogic.Init();
            MenuLogic.Init();
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
