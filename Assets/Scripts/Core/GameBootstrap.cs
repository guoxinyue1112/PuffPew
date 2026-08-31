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
        if (FindAnyObjectByType<GameManager>() != null)
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
        PuffPewAudio.Initialize();

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
        enemySpawner.Initialize(playerObject.transform, PuffPewArt.GetEnemySprite() != null ? PuffPewArt.GetEnemySprite() : squareSprite);
        waveManager.Initialize(enemyManager, enemySpawner);

        GameObject uiRoot = CreateCanvasRoot();
        HUDController hudController = uiRoot.GetComponent<HUDController>();
        if (hudController == null)
        {
            hudController = uiRoot.AddComponent<HUDController>();
        }

        LevelUpUI levelUpUI = uiRoot.GetComponent<LevelUpUI>();
        if (levelUpUI == null)
        {
            levelUpUI = uiRoot.AddComponent<LevelUpUI>();
        }

        WeaponChoiceUI weaponChoiceUI = uiRoot.GetComponent<WeaponChoiceUI>();
        if (weaponChoiceUI == null)
        {
            weaponChoiceUI = uiRoot.AddComponent<WeaponChoiceUI>();
        }

        EndGameUI endGameUI = uiRoot.GetComponent<EndGameUI>();
        if (endGameUI == null)
        {
            endGameUI = uiRoot.AddComponent<EndGameUI>();
        }

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
        Sprite playerSprite = PuffPewArt.GetPlayerRightSprite();
        spriteRenderer.sprite = playerSprite != null ? playerSprite : squareSprite;
        spriteRenderer.color = playerSprite != null ? Color.white : new Color(0.35f, 0.85f, 1f);
        spriteRenderer.sortingOrder = 10;
        if (playerSprite == null)
        {
            playerObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }

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
        playerObject.AddComponent<PlayerVisual>();

        return playerObject;
    }

    private static void CreateBackground(Sprite squareSprite)
    {
        GameObject background = new("Background");
        background.transform.position = Vector3.zero;

        SpriteRenderer renderer = background.AddComponent<SpriteRenderer>();
        Sprite backgroundSprite = PuffPewArt.GetBackgroundSprite();
        renderer.sprite = backgroundSprite != null ? backgroundSprite : squareSprite;
        renderer.color = backgroundSprite != null ? Color.white : new Color(0.16f, 0.21f, 0.18f);
        renderer.sortingOrder = -100;

        if (backgroundSprite != null)
        {
            PuffPewArt.SetCoverWorldSize(background.transform, backgroundSprite, 36f, 20f);
        }
        else
        {
            background.transform.localScale = new Vector3(40f, 24f, 1f);
        }
    }

    private static void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() != null)
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
        GameObject canvasObject = GameObject.Find("UI");
        if (canvasObject == null)
        {
            canvasObject = new GameObject("UI");
        }

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = canvasObject.AddComponent<Canvas>();
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = canvasObject.AddComponent<CanvasScaler>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        if (canvasObject.GetComponent<GraphicRaycaster>() == null)
        {
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        TMP_Settings settings = TMP_Settings.instance;
        if (settings == null)
        {
            Debug.LogWarning("TextMeshPro settings asset is missing. Import TMP Essentials if text does not render.");
        }

        return canvasObject;
    }
}
