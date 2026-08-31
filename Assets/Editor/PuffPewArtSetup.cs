#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PuffPewArtSetup
{
    private const string ResourcesRoot = "Assets/Resources";
    private const string ConfigAssetPath = "Assets/Resources/PuffPewArtConfig.asset";
    private const string BackgroundPath = "Assets/Resources/background.png";
    private const string PlayerTexturePath = "Assets/Resources/Player.png";
    private const string EnemyPath = "Assets/Resources/anemy.png";
    private const string BulletPath = "Assets/Resources/bullet.png";
    private const string PistolPath = "Assets/Resources/gun.png";
    private const string AxePath = "Assets/Resources/axe.png";
    private const string BombPath = "Assets/Resources/bomb.png";
    private const string PlayerLeftName = "Player_Left";
    private const string PlayerRightName = "Player_Right";

    [MenuItem("Tools/PuffPew/Setup Art Assets")]
    public static void SetupArtAssets()
    {
        PuffPewSetup.EnsureProjectSetup();

        List<string> pngPaths = Directory.Exists(ResourcesRoot)
            ? Directory.GetFiles(ResourcesRoot, "*.png", SearchOption.AllDirectories).ToList()
            : new List<string>();

        if (pngPaths.Count == 0)
        {
            Debug.LogWarning("PuffPew Art Setup: no PNG files found under Assets/Resources.");
            return;
        }

        foreach (string path in pngPaths)
        {
            ConfigureImporter(path, string.Equals(path.Replace("\\", "/"), PlayerTexturePath, StringComparison.OrdinalIgnoreCase));
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        PuffPewArtConfig config = LoadOrCreateConfig();
        PopulateConfig(config);
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssetIfDirty(config);

        SaveGameScene();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(BuildReport(config, pngPaths));
    }

    private static void ConfigureImporter(string assetPath, bool isPlayerTexture)
    {
        if (isPlayerTexture)
        {
            return;
        }

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null)
        {
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.alphaIsTransparency = true;
        importer.filterMode = FilterMode.Bilinear;
        importer.spritePixelsPerUnit = 100f;
        importer.mipmapEnabled = false;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.crunchedCompression = false;

        importer.spriteImportMode = SpriteImportMode.Single;

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
    }

    private static PuffPewArtConfig LoadOrCreateConfig()
    {
        PuffPewArtConfig config = AssetDatabase.LoadAssetAtPath<PuffPewArtConfig>(ConfigAssetPath);
        if (config != null)
        {
            return config;
        }

        config = ScriptableObject.CreateInstance<PuffPewArtConfig>();
        AssetDatabase.CreateAsset(config, ConfigAssetPath);
        return config;
    }

    private static void PopulateConfig(PuffPewArtConfig config)
    {
        config.backgroundPath = File.Exists(BackgroundPath) ? BackgroundPath : null;
        config.playerTexturePath = File.Exists(PlayerTexturePath) ? PlayerTexturePath : null;
        config.enemyPath = File.Exists(EnemyPath) ? EnemyPath : null;
        config.bulletPath = File.Exists(BulletPath) ? BulletPath : null;
        config.pistolPath = File.Exists(PistolPath) ? PistolPath : null;
        config.axePath = File.Exists(AxePath) ? AxePath : null;
        config.bombPath = File.Exists(BombPath) ? BombPath : null;

        config.backgroundSprite = LoadSingleSprite(config.backgroundPath);
        config.enemySprite = LoadSingleSprite(config.enemyPath);
        config.bulletSprite = LoadSingleSprite(config.bulletPath);
        config.pistolSprite = LoadSingleSprite(config.pistolPath);
        config.axeSprite = LoadSingleSprite(config.axePath);
        config.bombSprite = LoadSingleSprite(config.bombPath);

        if (!string.IsNullOrEmpty(config.playerTexturePath))
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(config.playerTexturePath).OfType<Sprite>().ToArray();
            config.playerLeftSprite = FindExactSprite(sprites, PlayerLeftName);
            config.playerRightSprite = FindExactSprite(sprites, PlayerRightName);

            if (config.playerLeftSprite == null || config.playerRightSprite == null)
            {
                Debug.LogWarning(
                    "PuffPew Art Setup: player slices were not found by name. " +
                    "Expected Player_Left / Player_Right on Assets/Resources/Player.png. " +
                    "Player sprites were left unmodified.");
            }
        }
        else
        {
            config.playerLeftSprite = null;
            config.playerRightSprite = null;
        }
    }

    private static Sprite FindExactSprite(IEnumerable<Sprite> sprites, params string[] names)
    {
        foreach (string name in names)
        {
            Sprite match = sprites.FirstOrDefault(sprite =>
                string.Equals(sprite.name, name, StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                return match;
            }
        }

        return null;
    }

    private static Sprite LoadSingleSprite(string assetPath)
    {
        return string.IsNullOrEmpty(assetPath) ? null : AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
    }

    private static void SaveGameScene()
    {
        string gameScenePath = "Assets/Scenes/GameScene.unity";
        if (!File.Exists(gameScenePath))
        {
            return;
        }

        var scene = EditorSceneManager.OpenScene(gameScenePath);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static string BuildReport(PuffPewArtConfig config, IEnumerable<string> pngPaths)
    {
        List<string> lines = new()
        {
            "PuffPew Art Setup complete.",
            "Detected PNGs:"
        };

        lines.AddRange(pngPaths.Select(path => $"- {path.Replace("\\", "/")}"));
        lines.Add("Bindings:");
        lines.Add($"- Background <- {config.backgroundPath}");
        lines.Add($"- Player texture <- {config.playerTexturePath}");
        lines.Add($"- Enemy <- {config.enemyPath}");
        lines.Add($"- Bullet <- {config.bulletPath}");
        lines.Add($"- Pistol <- {config.pistolPath}");
        lines.Add($"- Axe <- {config.axePath}");
        lines.Add($"- Bomb <- {config.bombPath}");
        lines.Add($"- Player_Left sprite assigned: {config.playerLeftSprite != null}");
        lines.Add($"- Player_Right sprite assigned: {config.playerRightSprite != null}");
        return string.Join("\n", lines);
    }
}
#endif
