using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("MENU")]
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject creditsText;
    public GameObject backButton;
    public GameObject exitButton;

    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip musicToPlay;

    public void Start()
    {
        MainMenuButton();
        backButton.SetActive(false);
        creditsText.SetActive(false); 

        if (audioSource != null && musicToPlay != null)
        {
            audioSource.clip = musicToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or Music is not assigned.");
        }
    }

    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void CreditsButton()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        backButton.SetActive(true);
        creditsText.SetActive(true); 
        exitButton.SetActive(false);
    }

    public void MainMenuButton()
    {

        MainMenu.SetActive(true);
        CreditsMenu.SetActive(true);
        creditsText.SetActive(false);
        backButton.SetActive(false);
        exitButton.SetActive(true);
        
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
