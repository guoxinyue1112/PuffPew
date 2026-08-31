#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PuffPewSetup
{
    private const string SampleScenePath = "Assets/Scenes/SampleScene.unity";
    private const string GameScenePath = "Assets/Scenes/GameScene.unity";

    static PuffPewSetup()
    {
        EditorApplication.delayCall += EnsureProjectSetup;
    }

    [MenuItem("Tools/PuffPew/Ensure GameScene")]
    public static void EnsureProjectSetup()
    {
        EnsureGameSceneAsset();
        EnsureBuildSettings();
        AssetDatabase.Refresh();
    }

    private static void EnsureGameSceneAsset()
    {
        if (File.Exists(GameScenePath) || !File.Exists(SampleScenePath))
        {
            return;
        }

        File.Copy(SampleScenePath, GameScenePath);
        AssetDatabase.ImportAsset(GameScenePath);
        EditorSceneManager.MarkAllScenesDirty();
    }

    private static void EnsureBuildSettings()
    {
        EditorBuildSettingsScene[] currentScenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in currentScenes)
        {
            if (scene.path == GameScenePath)
            {
                return;
            }
        }

        EditorBuildSettingsScene[] updatedScenes = new EditorBuildSettingsScene[currentScenes.Length + 1];
        for (int i = 0; i < currentScenes.Length; i++)
        {
            updatedScenes[i] = currentScenes[i];
        }

        updatedScenes[^1] = new EditorBuildSettingsScene(GameScenePath, true);
        EditorBuildSettings.scenes = updatedScenes;
    }
}
#endif
