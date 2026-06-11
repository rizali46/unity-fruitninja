using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 1. Required for restarting the scene!

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Components")]
    public TMP_Text scoreText;
    public TMP_Text missesText;      // Drag Misses Text here
    public GameObject gameOverPanel; // Drag Game Over Panel here

    [Header("Game Settings")]
    public int maxMisses = 10;

    private int currentScore = 0;
    private int currentMisses = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // Ensure time is running normally and the panel is hidden on start
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public void AddPoints(int pointsToAdd)
    {
        if (isGameOver) return; // Stop scoring if dead

        currentScore += pointsToAdd;
        UpdateUI();
    }

    // New method to handle missing a fruit
    public void RegisterMiss()
    {
        if (isGameOver) return;

        currentMisses++;
        UpdateUI();

        if (currentMisses >= maxMisses)
        {
            GameOver();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (missesText != null) missesText.text = "Misses: " + currentMisses + " / " + maxMisses;
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true); // Show the game over screen

        // This instantly freezes all Unity physics, stopping the cubes and spawner!
        Time.timeScale = 0f;
    }

    // The Restart button will call this
    public void RestartGame()
    {
        // Reloads the exact scene we are currently playing
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}