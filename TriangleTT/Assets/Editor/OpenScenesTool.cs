using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class OpenScenesTool {
    [MenuItem("Tools/1. Open scenes (full game setup)")]
    public static void OpenScenes() {
        SceneSetup[] setup = new SceneSetup[] {
            new() { path = "Assets/Scenes/MainScene.unity", isLoaded = true, isActive = true },
            new() { path = "Assets/Scenes/UIScene.unity", isLoaded = true, isActive = false },
        };
        EditorSceneManager.RestoreSceneManagerSetup(setup);
    }

    [MenuItem("Tools/2. Open game scene")]
    public static void OpenGameScene() {
        SceneSetup[] setup = new SceneSetup[] {
            new() { path = "Assets/Scenes/MainScene.unity", isLoaded = true, isActive = true },
        };
        EditorSceneManager.RestoreSceneManagerSetup(setup);
    }

    [MenuItem("Tools/3. Open UI scene")]
    public static void OpenUIScene() {
        SceneSetup[] setup = new SceneSetup[] {
            new() { path = "Assets/Scenes/UIScene.unity", isLoaded = true, isActive = true },
        };
        EditorSceneManager.RestoreSceneManagerSetup(setup);
    }
}
