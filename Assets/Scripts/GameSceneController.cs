using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameSceneController : MonoBehaviour
{
    public event EnemyDestroyedHandler ScoreUpdatedOnKill;
    public event Action<int> LifeLost;
    public event Action Lost;
    #region Field Declarations

    [Header("Enemy & Power Prefabs")]
    [Space]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private ShipMove playerShip;
    [SerializeField] private PowerupController[] powerUpPrefabs;

    [Header("Level Definitions")]
    [Space]
    public List<LevelDefinition> levels;
    [HideInInspector] public LevelDefinition currentLevel;

    [Header("Player ship settings")]
    [Space]
    [Range(3, 8)]
    private float playerSpeed = 5f;
    [Range(45,135)]
    private float playerTurnSpeed = 90f;
    [Range(1, 10)]
    public float shieldDuration = 3;
    [Range(1, 10)]
    public float X2Duration = 10;

    private int totalPoints;
    private int lives = 3;

    private int currentLevelIndex = 0;
    private WaitForSeconds shipSpawnDelay = new WaitForSeconds(2);

    #endregion

    #region Startup

    void Awake()
    {
        StartLevel(currentLevelIndex);
    }

    #endregion

    #region Level Management

    private void StartLevel(int levelIndex)
    {
        currentLevel = levels[levelIndex];

        StartCoroutine(SpawnShip(false));
        StartCoroutine(SpawnEnemies());

        if (currentLevel.hasPowerUps)
            StartCoroutine(SpawnPowerUp());
    }

    private void EndLevel()
    {
        currentLevelIndex++;
        StopAllCoroutines();

        //If last level the game over
        if (currentLevelIndex < levels.Count)
        {
            //TODO: Clean up
            StartLevel(currentLevelIndex);
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
        ship.GetComponent<ShieldController>().shieldDuration = shieldDuration;
        ship.GetComponent<ShieldController>().X2Duration = X2Duration;
        ship.GetComponent<ShipHealth>().HitByEnemy += Ship_HitByEnemy;
        yield return null;
    }

    private void Ship_HitByEnemy()
    {
        lives--;
        LifeLost?.Invoke(lives);
        if (lives > 0)
            StartCoroutine(SpawnShip(true));
        else
        {
            Lost?.Invoke();
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

            enemy.EnemyDestroyed += Enemy_EnemyDestroyed;
            yield return wait;
        }
    }

    private void Enemy_EnemyDestroyed(int pointValue)
    {
        totalPoints += pointValue;
        if (ScoreUpdatedOnKill != null)
        {
            ScoreUpdatedOnKill(totalPoints);
        }

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
}
