using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> musicList;
    private int currentTrackIndex = 0;


    [Header("LEVEL")]
    public GameObject pausePanel; // Pause panel game object
    private bool isPaused; // Flag whether the game is paused


    void Start()
    {
        if (musicList.Count > 0 && audioSource != null)
        {
            PlayCurrentTrack();
        }
        else
        {
            Debug.LogError("Music list is empty or AudioSource is not assigned.");
        }

        pausePanel.SetActive(false); // Close the pause panel at the beginning
        isPaused = false;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }

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

        private void PlayCurrentTrack()
    {
        audioSource.clip = musicList[currentTrackIndex];
        audioSource.Play();
    }

    private void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicList.Count;
        PlayCurrentTrack();
    }

        public void NextTrackButtonClicked()
    {
        PlayNextTrack();
    }


}
