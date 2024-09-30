using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Text box for displaying the current score
    [SerializeField] private GameObject floatingScorePrefab; // Floating score prefab, to achieve the floating effect of the score on the screen
    [SerializeField] private Transform floatingScoreSpawnPoint; // The position where the floating score appears
    [SerializeField] private float floatingScoreInstanceTime = 2f; // Destroy the floating score instance after "X" seconds

    private int currentScore = 0; // Current player's score, initialized to 0

    private void Start()
    {
        UpdateScoreText(); // When the game starts, update the score display
    }

    public void AddScore(int points)
    {
        currentScore += points; // Player score, increase the current score
        UpdateScoreText(); // Update the score display
        ShowFloatingScore(points); // Show score animation
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{currentScore}"; // Display the current score on the UI
    }

    private void ShowFloatingScore(int points)
    {
        GameObject floatingScoreInstance = Instantiate(floatingScorePrefab, floatingScoreSpawnPoint.position, Quaternion.identity, floatingScoreSpawnPoint); // Create a floating score instance
        TextMeshProUGUI floatingScoreText = floatingScoreInstance.GetComponent<TextMeshProUGUI>(); // Get the Text component of the floating score
        floatingScoreText.text = $"+{points}"; // Set the text of the floating score to the score value
        Destroy(floatingScoreInstance, floatingScoreInstanceTime); // Destroy the floating score instance after 2 seconds
    }
}
