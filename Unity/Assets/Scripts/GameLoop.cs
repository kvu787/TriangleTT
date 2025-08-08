using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrivingGameV2 {
    public class GameLoop : MonoBehaviour {
        private bool isInitialized = false;

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

            Screen.SetResolution(SceneSettings.StartingResolution.x, SceneSettings.StartingResolution.y, FullScreenMode.Windowed);

            Input.Init();
            VSyncLogic.Init();
            MaxQueuedFramesLogic.Init();
            CheckpointLogic.Init();
            CarSwitchLogic.InitCars();
            CarLogic.Init();
            CarResetLogic.Init();
            MenuLogic.Init();
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
