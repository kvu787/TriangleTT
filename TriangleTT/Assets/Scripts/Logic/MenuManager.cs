using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace TriangleTT {
    public static class MenuManager {
        private static readonly List<GameObject> Cones = new();
        private static readonly List<GameObject> Barriers = new();

        public static void Init() {
            SceneObjects.Menu.SetActive(false);

            SceneObjects.LapTimesFilePathInputField.text = LapTimer.LapTimesFilePath;
            SceneObjects.DebugLogFilePathInputField.text = $"{Application.persistentDataPath}/Player.log".Replace("/", "\\");

            SceneObjects.OpenMenuButton.onClick.AddListener(() => SceneObjects.Menu.SetActive(true));
            SceneObjects.CloseMenuButton.onClick.AddListener(() => SceneObjects.Menu.SetActive(false));

            SetShadowMode(SceneObjects.ShadowToggle.isOn);
            SceneObjects.ShadowToggle.onValueChanged.AddListener(enable => SetShadowMode(enable));

            QualitySettings.vSyncCount = SceneObjects.VSyncDropdown.value;
            SceneObjects.VSyncDropdown.onValueChanged.AddListener(index => QualitySettings.vSyncCount = index);

            UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            Assert.IsNotNull(urpAsset);
            urpAsset.renderScale = SceneObjects.RenderScaleSlider.value;
            SceneObjects.RenderScaleSlider.onValueChanged.AddListener(value => urpAsset.renderScale = value);

            SetupGameObjectToggle(Cones, Tag.Cone, SceneObjects.EnableConesToggle);
            SetupGameObjectToggle(Barriers, Tag.Barrier, SceneObjects.EnableBarriersToggle);

            FpsDisplayManager.SetSlowFpsCounter(SceneObjects.SlowDownFpsCounterToggle.isOn);
            SceneObjects.SlowDownFpsCounterToggle.onValueChanged.AddListener(enable => FpsDisplayManager.SetSlowFpsCounter(enable));
        }

        private static void SetupGameObjectToggle(List<GameObject> gameObjects, Tag tag, Toggle toggle) {
            gameObjects.AddRange(GameObject.FindGameObjectsWithTag(tag.ToString()));
            SetGameObjectsState(toggle.isOn, gameObjects);
            toggle.onValueChanged.AddListener(enable => SetGameObjectsState(enable, gameObjects));
        }

        private static void SetGameObjectsState(bool enable, List<GameObject> gameObjects) {
            foreach (GameObject gameObject in gameObjects) {
                gameObject.SetActive(enable);
            }
        }

        private static void SetShadowMode(bool enable) {
            if (enable) {
                SceneObjects.Sunlight.shadows = LightShadows.Hard;
            } else {
                SceneObjects.Sunlight.shadows = LightShadows.None;
            }
        }
    }
}
