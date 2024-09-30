using System.Collections;
using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundText; // Display the text of the current round
    [SerializeField] AudioClip roundAudioClip; // Round sound effects
    [SerializeField] float roundTextDisplayTime = 1.0f; // Round text display duration
    [SerializeField] float roundTextFadeDuration = 2.0f; // Round text fade-out duration

    private CanvasGroup canvasGroup; // CanvasGroup component of UI element

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // Get CanvasGroup component
    }

    public IEnumerator ShowRoundText(int currentRound)
    {
        roundText.text = "Round " + currentRound; // Update round display text
        AudioSource.PlayClipAtPoint(roundAudioClip, Camera.main.transform.position); // Play round sound effects

        canvasGroup.alpha = 1; // Set UI transparency to 1 (display)
        yield return new WaitForSeconds(roundTextDisplayTime); // Wait for the specified time

        float elapsedTime = 0;
        while (elapsedTime < roundTextFadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / roundTextFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0; // Set UI transparency to 0 (hide)
    }
}
