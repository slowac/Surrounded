using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel; // Your TutorialPanel object

    void Start()
    {
        tutorialPanel.SetActive(false); // Hide TutorialPanel at start
    }

    public void ShowTutorial() // Show TutorialPanel
    {
        tutorialPanel.SetActive(true);

        Cursor.visible = true; // Show mouse
        Cursor.lockState = CursorLockMode.None; // Unlock mouse

        Time.timeScale = 0; // Pause game
    }

    public void CloseTutorial() // Close TutorialPanel
    {
        tutorialPanel.SetActive(false);

        Cursor.visible = false; // Hide mouse
        Cursor.lockState = CursorLockMode.Locked; // Lock mouse

        Time.timeScale = 1; // Resume game
    }
}
