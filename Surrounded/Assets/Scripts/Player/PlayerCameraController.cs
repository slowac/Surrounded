using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameObject player; // Player object
    [SerializeField] private float lookSensitivity; // Mouse sensitivity
    [SerializeField] private float smoothing; // Smooth movement value
    [SerializeField] private int maxLookRotation; // Maximum vertical rotation angle

    private Vector2 smoothedVelocity; // Smooth velocity
    private Vector2 currentLookingPos; // Current observation position
    private float recoilRecoveryDelay = 0.1f; // Control the delay time for recovering recoil (unit: seconds)
    private Vector2 initialLookingPos; // Record the initial position of the camera before the recoil is generated

    private Vector3 lastPosition; // Record the position of the player in the previous frame
    private bool hasMoved = false; // Record whether the player has moved

    public TutorialPanelController tutorialPanelController; // Your TutorialPanelController object
    public Vector2 RecoilOffset { get; set; }
    public float RecoilRecoverySpeed { get; set; } // Recoil recovery speed

    public void ApplyRecoilWithRecovery(Vector2 recoil, float recoverySpeed, Vector2 initialPos)
    {
        initialLookingPos = initialPos; //Record the initial position of the camera before the recoil
        RecoilOffset += recoil;
        RecoilRecoverySpeed = recoverySpeed;
        StartCoroutine(RecoverRecoilAfterDelay(recoilRecoveryDelay));
    }

    //Add a new method to get the current camera position from the outside
    public Vector2 GetCurrentLookingPos()
    {
        return currentLookingPos;
    }

    private IEnumerator RecoverRecoilAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (RecoilOffset != Vector2.zero)
        {
            RecoilOffset = Vector2.MoveTowards(RecoilOffset, Vector2.zero, RecoilRecoverySpeed * Time.deltaTime);
            currentLookingPos = Vector2.Lerp(currentLookingPos, initialLookingPos, RecoilRecoverySpeed * Time.deltaTime); // Restore to the recorded camera state
            yield return null;
        }
    }

    private void Start()
    {
        player = transform.parent.gameObject != null ? transform.parent.gameObject : gameObject;
        lastPosition = transform.position; // Initialize lastPosition
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor
        Cursor.visible = false; // Hide the mouse cursor
    }

    private void Update()
    {
        RotateCamera(); // Rotate the camera every frame

        // Check if the player has moved
        if (transform.position != lastPosition && !hasMoved)
        {
            hasMoved = true; // Mark that the player has moved
            tutorialPanelController.ShowTutorial(); // Show the tutorial
        }

        // Check if the player has pressed the "H" key
        if (Input.GetKeyDown(KeyCode.H))
        {
            tutorialPanelController.ShowTutorial(); // Show the tutorial
        }

        lastPosition = transform.position; // Update the player position
    }

    private void LateUpdate()
    {
        // Update the camera position
        transform.position = new Vector3(player.transform.position.x,
        player.transform.position.y + 0.48f,
        player.transform.position.z);
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"),
        Input.GetAxisRaw("Mouse Y")); // Get the mouse input value

        inputValues = Vector2.Scale(inputValues,
        new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));
        // Adjust the relationship between the input value and the sensitivity and smoothing value

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);
        // Smooth the input value

        currentLookingPos += smoothedVelocity + RecoilOffset; // Update the current observation position

        // Calculate the speed of recoil recovery
        Vector2 recoverySpeed = RecoilOffset * RecoilRecoverySpeed * Time.deltaTime;

        // Gradually recover the recoil of the camera
        RecoilOffset -= recoverySpeed;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -maxLookRotation, maxLookRotation); // Limit the vertical rotation angle

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, player.transform.up);
        // Rotate the camera and player

        //Debug.Log($"Mouse Input: {inputValues}, Smoothed Velocity: {smoothedVelocity}, Current Looking Position: {currentLookingPos}");
    }
}