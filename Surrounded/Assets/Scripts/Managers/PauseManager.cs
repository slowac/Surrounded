using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Pause panel game object
    private bool isPaused; // Flag whether the game is paused

    private void Start()
    {
        pausePanel.SetActive(false); // Close the pause panel at the beginning
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // When the Esc key is pressed
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Set the game time scale to 0 and pause the game
        pausePanel.SetActive(true); // Activate the pause panel
        isPaused = true;

        // Unlock the mouse and display the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Set the game time scale to 1 and resume the game
        pausePanel.SetActive(false); // Close the pause panel
        isPaused = false;

        // Lock the mouse and hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        // Set all active game objects in the current scene to inactive state
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(false);
        }
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}