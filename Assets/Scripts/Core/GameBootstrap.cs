using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public class GameBootstrap : MonoBehaviour
{
    private const float ArenaMinX = -18f;
    private const float ArenaMaxX = 18f;
    private const float ArenaMinY = -10f;
    private const float ArenaMaxY = 10f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoBootstrap()
    {
        if (FindFirstObjectByType<GameManager>() != null)
        {
            return;
        }

        GameObject bootstrapObject = new("PuffPewBootstrap");
        bootstrapObject.AddComponent<GameBootstrap>();
    }

    private void Start()
    {
        BuildGame();
    }

    private void BuildGame()
    {
        Sprite squareSprite = RuntimeSpriteFactory.GetSquareSprite();

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObject = new("Main Camera");
            mainCamera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            cameraObject.AddComponent<AudioListener>();
        }

        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 11f;
        mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        mainCamera.backgroundColor = new Color(0.10f, 0.13f, 0.17f);

        CreateBackground(squareSprite);
        EnsureEventSystem();

        GameObject systemsRoot = new("Systems");
        EnemyManager enemyManager = systemsRoot.AddComponent<EnemyManager>();
        WaveManager waveManager = systemsRoot.AddComponent<WaveManager>();
        GameManager gameManager = systemsRoot.AddComponent<GameManager>();

        FloatingTextWorld floatingTextWorld = new GameObject("FloatingTextWorld").AddComponent<FloatingTextWorld>();

        GameObject playerObject = CreatePlayer(squareSprite);
        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        PlayerExperience playerExperience = playerObject.GetComponent<PlayerExperience>();
        WeaponManager weaponManager = playerObject.GetComponent<WeaponManager>();

        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
        }

        cameraFollow.Initialize(new Vector3(0f, 0f, -10f));

        GameObject enemySpawnerObject = new("EnemySpawner");
        EnemySpawner enemySpawner = enemySpawnerObject.AddComponent<EnemySpawner>();
        enemySpawner.Initialize(playerObject.transform, squareSprite);
        waveManager.Initialize(enemyManager, enemySpawner);

        GameObject uiRoot = CreateCanvasRoot();
        HUDController hudController = uiRoot.AddComponent<HUDController>();
        LevelUpUI levelUpUI = uiRoot.AddComponent<LevelUpUI>();
        WeaponChoiceUI weaponChoiceUI = uiRoot.AddComponent<WeaponChoiceUI>();
        EndGameUI endGameUI = uiRoot.AddComponent<EndGameUI>();

        hudController.BuildUI();
        levelUpUI.BuildUI();
        weaponChoiceUI.BuildUI();
        endGameUI.BuildUI();

        gameManager.Initialize(
            playerStats,
            playerExperience,
            weaponManager,
            waveManager,
            enemyManager,
            hudController,
            levelUpUI,
            weaponChoiceUI,
            endGameUI,
            floatingTextWorld);
    }

    private static GameObject CreatePlayer(Sprite squareSprite)
    {
        GameObject playerObject = new("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;

        SpriteRenderer spriteRenderer = playerObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = squareSprite;
        spriteRenderer.color = new Color(0.35f, 0.85f, 1f);
        playerObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        Rigidbody2D rigidbody2D = playerObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.linearDamping = 6f;
        rigidbody2D.freezeRotation = true;

        CircleCollider2D circleCollider = playerObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f;

        PlayerStats playerStats = playerObject.AddComponent<PlayerStats>();
        playerObject.AddComponent<PlayerExperience>();
        PlayerController playerController = playerObject.AddComponent<PlayerController>();
        playerController.InitializeBounds(ArenaMinX, ArenaMaxX, ArenaMinY, ArenaMaxY);
        playerObject.AddComponent<WeaponManager>();

        return playerObject;
    }

    private static void CreateBackground(Sprite squareSprite)
    {
        GameObject background = new("Background");
        background.transform.position = new Vector3(0f, 0f, 10f);
        background.transform.localScale = new Vector3(40f, 24f, 1f);

        SpriteRenderer renderer = background.AddComponent<SpriteRenderer>();
        renderer.sprite = squareSprite;
        renderer.color = new Color(0.16f, 0.21f, 0.18f);
        renderer.sortingOrder = -10;
    }

    private static void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
#else
        eventSystemObject.AddComponent<StandaloneInputModule>();
#endif
    }

    private static GameObject CreateCanvasRoot()
    {
        GameObject canvasObject = new("UI");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.AddComponent<GraphicRaycaster>();

        TMP_Settings settings = TMP_Settings.instance;
        if (settings == null)
        {
            Debug.LogWarning("TextMeshPro settings asset is missing. Import TMP Essentials if text does not render.");
        }

        return canvasObject;
    }
}
