using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TriangleTT {
    public static class MenuManager {
        private static List<GameObject> Cones;

        public static void Init() {
            SceneObjects.Menu.SetActive(false);

            SceneObjects.LapTimesFilePathInputField.text = Checkpointer.LapTimesFilePath;
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

            Cones = GameObject.FindGameObjectsWithTag(Tags.Cone.ToString()).ToList();
            SetConesState(SceneObjects.EnableConesToggle.isOn);
            SceneObjects.EnableConesToggle.onValueChanged.AddListener(enable => SetConesState(enable));
        }

        private static void SetConesState(bool enable) {
            foreach (GameObject cone in Cones) {
                cone.SetActive(enable);
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
