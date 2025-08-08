using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace TriangleTT {
    public class GameLoop : MonoBehaviour {
        private bool isInitialized = false;
        private readonly Stopwatch adjustAspectRatioTimer = new();

        void Awake() {
            Debug.Log($"GameLoop Awake on {this.gameObject.name} in scene {this.gameObject.scene.name}");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {
            if (!SceneManager.GetSceneByName("UIScene").isLoaded) {
                SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
                Debug.Log("Finished calling 'SceneManager.LoadScene(\"UIScene\", LoadSceneMode.Additive);'");
            }
        }

        // This can't be run in Start() because it depends on UIScene being fully loaded,
        // and I found out via testing that the UIScene isn't fully loaded until after
        // the Start() method exits.
        private void Init() {
            SceneObjects[] sceneObjectsScriptInstances = FindObjectsByType<SceneObjects>(FindObjectsSortMode.None);
            Debug.Log($"Found {sceneObjectsScriptInstances.Length} instances of {nameof(SceneObjects)}");
            foreach (SceneObjects sceneObjectsScriptInstance in sceneObjectsScriptInstances) {
                sceneObjectsScriptInstance.Init();
            }

            Input.Init();
            VSyncLogic.Init();
            MaxQueuedFramesLogic.Init();
            CheckpointLogic.Init();
            CarSwitchLogic.Init();
            CarLogic.Init();
            CarResetLogic.Init();
            MenuLogic.Init();
        }

        private void ForceAspectRatio() {
            if (this.adjustAspectRatioTimer.IsRunning) {
                if (this.adjustAspectRatioTimer.Elapsed > TimeSpan.FromSeconds(1)) {
                    int roundedWidth = Screen.width / 16 * 16;
                    int multiplier = roundedWidth / 16;
                    Screen.SetResolution(16 * multiplier, 9 * multiplier, FullScreenMode.Windowed);
                    this.adjustAspectRatioTimer.Reset();
                }
            } else {
                float targetRatio = 16f / 9f;
                float currentRatio = (float)Screen.width / Screen.height;
                if (!Mathf.Approximately(currentRatio, targetRatio)) {
                    this.adjustAspectRatioTimer.Start();
                }
            }
        }

        // Update is called once per frame
        void Update() {
            // TODO
            // Unity loads scenes asynchronously.
            // Theoretically, loading a scene can take a long time.
            // So, I should add a loading indicator if you wait for a scene to load.
            if (!SceneManager.GetSceneByName("UIScene").isLoaded) {
                Debug.Log("Update: UIScene not loaded yet");
                return;
            }

            if (!this.isInitialized) {
                Debug.Log("Running INIT...");
                this.Init();
                this.isInitialized = true;
            }

            this.ForceAspectRatio();

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
