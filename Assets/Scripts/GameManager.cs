using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        LOST,
        COMPLETE
    }


    public event Action<bool> ShowPauseMenu;
    public event Action<bool> ShowRestartButton;
    public event EnemyDestroyedHandler ScoreUpdatedOnKill;
    public event Action ShipSpawned;
    public event Action<int> LifeLost;
    public event Action<string> ShowCurrentStatus;
    #region Field Declarations

    [Header("Enemy & Power Prefabs")]
    [Space]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] ShipMove playerShip;
    [SerializeField] PowerUpController[] powerUpPrefabs;
    [HideInInspector] public bool isSpawnedEnemies;

    [Header("Level Definitions")]
    [Space]
    public List<LevelDefinition> levels;
    [HideInInspector] public LevelDefinition currentLevel;
    [HideInInspector] public int currentNumberOfEnemies;

    [Header("GameState")]
    [Space]
    public GameState _currentGameState = GameState.PREGAME;

    [Header("Player ship settings")]
    [Space]
    [Range(3, 8)]
    float playerSpeed = 5f;
    [Range(45, 135)]
    float playerTurnSpeed = 180f;
    [Range(1, 10)]
    public float shieldDuration = 3;
    [Range(1, 10)]
    public float X2Duration = 10;

    int totalPoints;
    int lives;

    public int currentLevelIndex = 0;
    WaitForSeconds shipSpawnDelay = new WaitForSeconds(2);

    #endregion
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    #region Startup

    private void Start()
    {
        _currentGameState = GameState.PREGAME;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    void Update()
    {
        if (_currentGameState == GameState.PREGAME)
            return;
        if (Input.GetKeyDown(KeyCode.Escape) && _currentGameState != GameState.LOST)
            Pause();
        //Debug.Log((true && false) + "!!!");
        //Debug.Log(enemiesDestroyed());
        //Debug.Log(isSpawnedEnemies);
        //Debug.Log(_currentGameState == GameState.RUNNING);
        //Debug.Log(currentNumberOfEnemies);
        //Debug.Log((enemiesDestroyed()) && (_currentGameState == GameState.RUNNING));
        if ((enemiesDestroyed()) && (_currentGameState == GameState.RUNNING))
        {
            //Debug.Log("AAAAAAAAAAAAAA");
            UpdateState(GameState.COMPLETE);
        }
    }
    #region Level Management

    public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene(1);
        _currentGameState = GameState.RUNNING;
        lives = 3;
        isSpawnedEnemies = false;
        HUDController.Instance.ResetShips();
        currentLevel = levels[levelIndex];
        currentNumberOfEnemies = currentLevel.numberOfEnemies;
        ShowCurrentStatus?.Invoke(currentLevel.levelName);
        ShowRestartButton?.Invoke(_currentGameState == GameState.LOST);
        ShowPauseMenu?.Invoke(_currentGameState == GameState.PREGAME);
        StartCoroutine(SpawnShip(true));
        StartCoroutine(SpawnEnemies());

        if (currentLevel.hasPowerUps)
            StartCoroutine(SpawnPowerUp());
    }

    private void LevelComplete()
    {
        ShowCurrentStatus?.Invoke("Level Complete");
        currentLevelIndex++;
        StopAllCoroutines();

        if (currentLevelIndex < levels.Count)
        {
            StartLevel(currentLevelIndex);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    #endregion

    #region Spawning

    private IEnumerator SpawnShip(bool delayed)
    {
        if (delayed)
            yield return shipSpawnDelay;

        ShipMove ship = Instantiate(playerShip, new Vector2(0, -4.67f), Quaternion.identity);
        ship.speed = playerSpeed;
        ship.turnSpeed = playerTurnSpeed;
        ship.GetComponent<PowerUpManagament>().shieldDuration = shieldDuration;
        ship.GetComponent<PowerUpManagament>().X2Duration = X2Duration;
        ship.GetComponent<ShipHealth>().DiedByEnemy += Died_HitByEnemy;
        ShipSpawned?.Invoke();
        yield return null;
    }
    private void Died_HitByEnemy()
    {
        lives--;
        LifeLost?.Invoke(lives);
        if (lives > 0)
            StartCoroutine(SpawnShip(true));
        else
        {
            HUDController.Instance.UpdateScore(0);
            UpdateState(GameState.LOST);
            ShowCurrentStatus?.Invoke("You lost");
        }
    }
    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(currentLevel.enemySpawnDelay);
        yield return wait;

        for (int i = 0; i < currentLevel.numberOfEnemies; i++)
        {
            Vector2 spawnPosition = ScreenBounds.RandomTopPosition();

            EnemyController enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.shotSpeed = currentLevel.enemyShotSpeed;
            enemy.speed = currentLevel.enemySpeed;
            enemy.shotdelayTime = currentLevel.enemyShotDelay;
            enemy.angerdelayTime = currentLevel.enemyAngerDelay;
            enemy.GetComponent<ShipHealth>().EnemyDestroyed+= Enemy_EnemyDestroyed;
            yield return wait;
        }
        isSpawnedEnemies = true;
    }
    private bool enemiesDestroyed()
    {
        return ((currentNumberOfEnemies == 0) && isSpawnedEnemies);
    }

    private void Enemy_EnemyDestroyed(int pointValue)
    {
        currentNumberOfEnemies--;
        totalPoints += pointValue;
        ScoreUpdatedOnKill?.Invoke(totalPoints);
    }
    private IEnumerator SpawnPowerUp()
    {
        while (true)
        {
            int index = UnityEngine.Random.Range(0, powerUpPrefabs.Length);
            Vector2 spawnPosition = ScreenBounds.RandomTopPosition();
            Instantiate(powerUpPrefabs[index], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(currentLevel.powerUpMinimumWait, currentLevel.powerUpMaximumWait));
        }
    }

    #endregion

    #region PauseMenu

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;
        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                StopAllCoroutines();
                StartLevel(currentLevelIndex);
                HUDController.Instance.UpdateScore(0);
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                ShowPauseMenu?.Invoke(_currentGameState == GameState.PAUSED);
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                ShowPauseMenu?.Invoke(_currentGameState == GameState.PAUSED);
                break;
            case GameState.LOST:
                Time.timeScale = 0.0f;
                ShowRestartButton?.Invoke(_currentGameState == GameState.LOST);
                break;
            case GameState.COMPLETE:
                Time.timeScale = 0.0f;
                LevelComplete();
                break;
            default:
                break;
        }
    }
    public void Pause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartLevel()
    {
        UpdateState(GameState.PREGAME);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
