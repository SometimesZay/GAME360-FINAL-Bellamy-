using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;

    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        // Subscribe early so we don't miss events
        EventManager.Subscribe("OnGameOver", OnGameOver);
        EventManager.Subscribe("OnPlayerDied", OnGameOver);
        EventManager.Subscribe("OnLevelComplete", OnVictory);
        EventManager.Subscribe("OnGameStart", HideAllMenus);

        // Subscribe for HUD updates
        EventManager.Subscribe("OnScoreChanged", OnScoreChanged);
    }

    void Start()
    {
        HideAllMenus();

        // Initialize HUD
        if (scoreText != null)
            scoreText.text = "Score: 0";
    }

    void Update()
    {
        // Timer updates every frame
        if (GameManager.Instance != null && timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(GameManager.Instance.GetTimeRemaining());
    }

    void OnDestroy()
    {
        // Unsubscribe from all events
        EventManager.Unsubscribe("OnGameOver", OnGameOver);
        EventManager.Unsubscribe("OnPlayerDied", OnGameOver);
        EventManager.Unsubscribe("OnLevelComplete", OnVictory);
        EventManager.Unsubscribe("OnGameStart", HideAllMenus);
        EventManager.Unsubscribe("OnScoreChanged", OnScoreChanged);
    }

    void HideAllMenus()
    {
        pauseMenu?.SetActive(false);
        gameOverMenu?.SetActive(false);
        victoryMenu?.SetActive(false);
    }

    void OnGameOver()
    {
        HideAllMenus();
        gameOverMenu?.SetActive(true);
        Debug.Log("🟥 Game Over Menu Shown");
    }

    void OnVictory()
    {
        HideAllMenus();
        victoryMenu?.SetActive(true);
        Debug.Log("🟩 Victory Menu Shown");
    }

    // === HUD Event Handlers ===
    void OnScoreChanged(object newScore)
    {
        if (scoreText == null)
            return;

        int score = (int)newScore;
        scoreText.text = "Score: " + score;
    }

    // === BUTTON HANDLERS ===
    public void OnResumeButton()
    {
        GameManager.Instance.ResumeGame();
        pauseMenu?.SetActive(false);
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // optional
    }
}




