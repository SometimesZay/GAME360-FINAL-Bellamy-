using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public Vector3 playerSpawnPoint = new Vector3(-8f, 0f, 0f);
    public float levelTimeLimit = 120f;

    private int score = 0;
    private float timeRemaining;
    private bool isGameActive = true;
    private bool isPaused = false;
    
    void Awake()
    {
        // Simple singleton - one per scene
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        // Debug current state
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("=== DEBUG ===");
            Debug.Log("Time: " + timeRemaining);
            Debug.Log("Active: " + isGameActive);
            Debug.Log("Paused: " + isPaused);
            Debug.Log("TimeScale: " + Time.timeScale);
        }

        // Only update timer when game is active AND not paused
        if (isGameActive && !isPaused)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver();
            }
        }

        // Restart input
        if (!isGameActive && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void InitializeGame()
    {
        Debug.Log("=== INITIALIZING GAME ===");

        // Reset all values
        score = 0;
        timeRemaining = levelTimeLimit;
        isGameActive = true;
        isPaused = false;

        // CRITICAL: Force unpause
        Time.timeScale = 1f;

        Debug.Log("Score: " + score);
        Debug.Log("Timer: " + timeRemaining);
        Debug.Log("Active: " + isGameActive);
        Debug.Log("Paused: " + isPaused);
        Debug.Log("TimeScale: " + Time.timeScale);

        EventManager.TriggerEvent("OnGameStart");
    }

    public void AddScore(int points)
    {
        score += points;
        EventManager.TriggerEvent("OnScoreChanged", score);
    }

    public void PlayerDied()
    {
        EventManager.TriggerEvent("OnPlayerDied");
    }

    public void LevelComplete()
    {
        isGameActive = false;
        PauseGame();
        EventManager.TriggerEvent("OnLevelComplete", score);
    }

    void GameOver()
    {
        Debug.Log("💀 GAME OVER");
        isGameActive = false;
        PauseGame();
        EventManager.TriggerEvent("OnGameOver", score);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("⏸️ Paused (TimeScale: " + Time.timeScale + ")");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("▶️ Resumed (TimeScale: " + Time.timeScale + ")");
    }

    public void RestartGame()
    {
        Debug.Log("=== RESTARTING ===");

        // Clear events
        EventManager.ClearAllEvents();

        // CRITICAL: Reset time scale
        Time.timeScale = 1f;
        isPaused = false;

        Debug.Log("TimeScale reset to: " + Time.timeScale);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetScore() => score;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsGameActive() => isGameActive;
    public bool IsPaused() => isPaused;
}
/*
using UnityEngine;
using UnityEngine.UI;
using TMPro; //Namesapce for textmeshpro
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    public int score = 0;//score is calculated
    public int lives = 3;
    public int enemiesKilled = 0;
    public int enemiesValue;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI enemiesKilledText;
    public GameObject gameOverPanel;
    //public TMP_Text scoreText;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUIReferences();
        UpdateUI();

    }

    private void Start()
    {
        //RefreshUIReferences();
        //UpdateUI();

    }

    private void RefreshUIReferences()
    {
        
       scoreText = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
       livesText = GameObject.Find("Lives")?.GetComponent<TextMeshProUGUI>();
       enemiesKilledText = GameObject.Find("EnemiesKilled")?.GetComponent<TextMeshProUGUI>();
       gameOverPanel = GameObject.Find("GameEndPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

    }
    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score increased by {points}. Total: {score}");
        UpdateUI();
    }


    public void LoseLife()
    {
        lives--;
        Debug.Log($"Life lost! Lives remaining: {lives}");
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        AddScore(enemiesValue); // Award points for kill
        Debug.Log($"Enemy killed! Total enemies defeated: {enemiesKilled}");
    }


    public void CollectiblePickedUp(int value)
    {
        AddScore(value);
        Debug.Log($"Collectible picked up worth {value} points!");
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (livesText) livesText.text = "Lives: " + lives;
        if (enemiesKilledText) enemiesKilledText.text = "EnemiesKilled: " + enemiesKilled;
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER!");
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void reloadGame()
    {
        //SceneManager.LoadScene("Delete");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Reset all game state
        score = 0;
        lives = 3;
        enemiesKilled = 0;

         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }

    private void DestroyAllGameObjects()
    {
        // Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Destroy all bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        // Destroy all collectibles
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject collectible in collectibles)
        {
            Destroy(collectible);
        }
    }
}
*/