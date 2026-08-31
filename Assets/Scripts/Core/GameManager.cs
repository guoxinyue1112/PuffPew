using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    LevelUp,
    WeaponChoice,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } = GameState.Playing;

    public PlayerStats PlayerStats { get; private set; }
    public PlayerExperience PlayerExperience { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public WaveManager WaveManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }

    private HUDController hudController;
    private LevelUpUI levelUpUI;
    private WeaponChoiceUI weaponChoiceUI;
    private EndGameUI endGameUI;
    private FloatingTextWorld floatingTextWorld;

    private bool pendingWeaponChoice;
    private bool pendingVictory;

    public bool GameplayRunning => State == GameState.Playing;
    public bool IsTerminalState => State == GameState.GameOver || State == GameState.Victory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    public void Initialize(
        PlayerStats playerStats,
        PlayerExperience playerExperience,
        WeaponManager weaponManager,
        WaveManager waveManager,
        EnemyManager enemyManager,
        HUDController hud,
        LevelUpUI levelUp,
        WeaponChoiceUI weaponChoice,
        EndGameUI endGame,
        FloatingTextWorld floatingTextRoot)
    {
        PlayerStats = playerStats;
        PlayerExperience = playerExperience;
        WeaponManager = weaponManager;
        WaveManager = waveManager;
        EnemyManager = enemyManager;
        hudController = hud;
        levelUpUI = levelUp;
        weaponChoiceUI = weaponChoice;
        endGameUI = endGame;
        floatingTextWorld = floatingTextRoot;

        hudController.Initialize(playerStats, playerExperience, weaponManager, waveManager, enemyManager);
        levelUpUI.Initialize(this);
        weaponChoiceUI.Initialize(this);
        endGameUI.Initialize(this);

        State = GameState.Playing;
        WaveManager.BeginFirstWave();
        hudController.Refresh();
    }

    public void NotifyPlayerDied()
    {
        if (IsTerminalState)
        {
            return;
        }

        State = GameState.GameOver;
        Time.timeScale = 0f;
        levelUpUI.Hide();
        weaponChoiceUI.Hide();
        endGameUI.ShowGameOver();
    }

    public void NotifyLevelUpAvailable()
    {
        if (IsTerminalState)
        {
            return;
        }

        EvaluateFlow();
    }

    public void NotifyWaveCompleted(int waveNumber)
    {
        if (IsTerminalState)
        {
            return;
        }

        if (waveNumber >= WaveManager.TotalWaves)
        {
            pendingVictory = true;
        }
        else
        {
            pendingWeaponChoice = true;
        }

        EvaluateFlow();
    }

    public void ResolveLevelUp(UpgradeType upgradeType)
    {
        if (PlayerExperience.PendingLevelUps <= 0 || IsTerminalState)
        {
            return;
        }

        PlayerStats.ApplyUpgrade(upgradeType);
        PlayerExperience.ConsumePendingLevelUp();
        levelUpUI.Hide();
        EvaluateFlow();
    }

    public void ResolveWeaponChoice(WeaponType weaponType)
    {
        if (!pendingWeaponChoice || IsTerminalState)
        {
            return;
        }

        pendingWeaponChoice = false;
        weaponChoiceUI.Hide();
        WeaponManager.AddWeapon(weaponType);
        ResumeGameplay();
        WaveManager.BeginNextWave();
    }

    public void EvaluateFlow()
    {
        if (IsTerminalState)
        {
            return;
        }

        if (PlayerStats.CurrentHP <= 0)
        {
            NotifyPlayerDied();
            return;
        }

        if (PlayerExperience.PendingLevelUps > 0)
        {
            PauseFor(GameState.LevelUp);
            weaponChoiceUI.Hide();
            levelUpUI.Show(PlayerStats);
            return;
        }

        if (pendingVictory)
        {
            pendingVictory = false;
            State = GameState.Victory;
            Time.timeScale = 0f;
            levelUpUI.Hide();
            weaponChoiceUI.Hide();
            endGameUI.ShowVictory();
            return;
        }

        if (pendingWeaponChoice)
        {
            PauseFor(GameState.WeaponChoice);
            levelUpUI.Hide();
            weaponChoiceUI.Show();
            return;
        }

        ResumeGameplay();
    }

    public void ResumeGameplay()
    {
        if (IsTerminalState)
        {
            return;
        }

        State = GameState.Playing;
        Time.timeScale = 1f;
        levelUpUI.Hide();
        weaponChoiceUI.Hide();
        hudController.Refresh();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnFloatingText(string message, Vector3 worldPosition, Color color)
    {
        if (floatingTextWorld != null)
        {
            floatingTextWorld.Spawn(message, worldPosition, color);
        }
    }

    private void PauseFor(GameState pauseState)
    {
        State = pauseState;
        Time.timeScale = 0f;
    }
}
