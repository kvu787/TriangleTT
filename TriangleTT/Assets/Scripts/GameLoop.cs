using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriangleTT {
    public class GameLoop : MonoBehaviour {
        private bool isInitialized = false;

        void Awake() {
            Debug.Log($"GameLoop Awake on {this.gameObject.name} in scene {this.gameObject.scene.name}");
            QualitySettings.maxQueuedFrames = 0;
            QualitySettings.vSyncCount = 1;
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
            Checkpointer.Init();
            CarSwitcher.Init();
            CarLogic.Init();
            CarResetter.Init();
            MenuManager.Init();
            CollisionLogic.Init();
        }

        // Update is called once per frame
        void Update() {
            // TODO
            // Unity loads scenes asynchronously.
            // Theoretically, loading a scene can take a long time.
            // So, I should add a loading indicator or screen.
            if (!SceneManager.GetSceneByName("UIScene").isLoaded) {
                Debug.Log("Update: UIScene not loaded yet");
                return;
            }

            if (!this.isInitialized) {
                Debug.Log("Running INIT...");
                this.Init();
                this.isInitialized = true;
            }

            AspectRatioEnforcer.Update();
            FpsDisplayManager.Update();
            Checkpointer.UpdateLapTimes();
            CarResetter.UpdateTimeout();

            if (CarResetter.IsTimedOut) {
                if (CarSwitcher.ProcessCarSwitch()) {
                    CarResetter.ResetCar();
                    CarLogic.WriteCarStateToCarObject();
                }
            } else {
                if (CarSwitcher.ProcessCarSwitch() || Input.ResetCarEvent || CollisionLogic.HasCarCollided()) {
                    CarResetter.ResetCar();
                    CarLogic.WriteCarStateToCarObject();
                    return;
                }

                CarLogic.ProcessCarInputAndPhysics();
                CarLogic.WriteCarStateToCarObject();
            }
        }
    }
}
