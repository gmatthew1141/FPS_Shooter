using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] playerSpawnPoints;

    [Header("UI")]
    [SerializeField] private GameObject playerDeadUI;
    [SerializeField] private Text scoreText;

    [Header("Enemies Waves")]
    [SerializeField] private Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;
    public SpawnState spawnState = SpawnState.COUNTING;

    [Header("Player")]
    [SerializeField] private Transform currPlayer;

    private List<GameObject> enemies;
    private float score;


    // Start is called before the first frame update
    void Start() {

        if (playerSpawnPoints.Length == 0) {
            Debug.LogWarning("No Player Spawn Point Available!");
        }

        if (enemySpawnPoints.Length == 0) {
            Debug.LogWarning("No Enemy Spawn Point Available!");
        }

        int index = GetRandomInt(playerSpawnPoints.Length - 1);
        currPlayer.position = playerSpawnPoints[index].position;
        enemies = new List<GameObject>();
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update() {
        if (spawnState == SpawnState.WAITING) {
            // check if enemy is alive
            Debug.Log("enemies alive: " + EnemyIsAlive());
            if (!EnemyIsAlive()) {
                Debug.Log("Wave completed");
                WaveCompleted();
            } else {
                return;
            }
        }

        if (waveCountdown <= 0) {
            if (spawnState != SpawnState.SPAWNING) {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
        
    }

    private void WaveCompleted() {
        Debug.Log("Wave completed");

        // start next round countdown
        spawnState = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1) {
            //nextWave = 0;
            Debug.Log("All waves completed!");
            // start next scene
        } else {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave) {

        spawnState = SpawnState.SPAWNING;
        for (int i = 0; i < wave.enemyCount.Length; i++) {
            for (int j = 0; j < wave.enemyCount[i]; j++) {
                Debug.Log("i: " + i);
                SpawnEnemy(enemyPrefabs[i], enemies.Count);
                yield return new WaitForSeconds(1 / wave.rate);
            }
        }
        
        spawnState = SpawnState.WAITING;
    }

    private void SpawnEnemy(GameObject enemy, int listIndex) {
        // spawn enemy
        Debug.Log("spawning enemy " + enemy.name);
        int index = GetRandomInt(enemySpawnPoints.Length - 1);
        var spawnOrigin = enemySpawnPoints[index].position;
        var spawnPoint = new Vector3(spawnOrigin.x + Random.Range(-10, 10), spawnOrigin.y, spawnOrigin.z + Random.Range(-10, 10));

        var _enemy = Instantiate(enemy, spawnPoint, enemySpawnPoints[index].rotation);

        enemies.Add(_enemy);
    }

    private bool EnemyIsAlive() {

        return enemies.Count == 0 ? false : true;
    }

    public void PlayerDead() {
        // enable Player dead UI
        playerDeadUI.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        // update score
    }

    public void RetryButton() {
        Debug.Log("Retry button pressed");
        // reset score
        score = 0;
        
        // destroy all remaining enemies
        foreach (var enemy in enemies) {
            enemies.Remove(enemy);
            Destroy(enemy);
        }

        // deactivate dead UI
        playerDeadUI.SetActive(false);

        // reset player status & spawn player in random spawn point
        int index = GetRandomInt(playerSpawnPoints.Length - 1);
        currPlayer.position = playerSpawnPoints[index].position;
        currPlayer.GetComponent<PlayerStatus>().ResetStatus();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1; 
    }

    public void ReportDead(GameObject enemy) {
        Debug.Log("report dead index: " + enemy);
        Debug.Log("enemies count in report dead: " + enemies.Count);
        enemies.Remove(enemy);
    }

    #region UI Buttons

    public void ReturnToMainButton() {
        // return player to main menu
    }

    private int GetRandomInt(int max) {
        return Random.Range(0, max);
    }

    #endregion

    #region Score Managerment Functions

    public float GetScore() {
        return score;
    }

    public void AddScore(float score) {
        this.score += score;
    }

    public void ResetScore() {
        score = 0;
    }

    #endregion

    [System.Serializable]
    public class Wave {
        public int[] enemyCount;
        public float rate;
    }

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

}

public enum EnemyType { SKELETON_AXE, SKELETON_BOMBER, SKELETON_AXE_THROWER,
    ARMORED_SKELETON_AXE, ARMORED_SKELETON_BOMBER, ARMORED_SKELETON_AXE_THROWER }

public enum PlayerState { ALIVE, DEAD }
