using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class OpenScenesTool {
    [MenuItem("Tools/Open Scenes")]
    public static void OpenScenes() {
        SceneSetup[] setup = new SceneSetup[] {
            new() { path = "Assets/Scenes/MainScene.unity", isLoaded = true, isActive = true },
            new() { path = "Assets/Scenes/UIScene.unity", isLoaded = true, isActive = false },
        };
        EditorSceneManager.RestoreSceneManagerSetup(setup);
    }
}
