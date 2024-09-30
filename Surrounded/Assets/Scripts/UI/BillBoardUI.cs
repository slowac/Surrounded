using UnityEngine;

namespace slowac_UI
{
    public class BillBoardUI : MonoBehaviour
    {
        private Camera playerCamera; // Player Camera

        private void Start()
        {
            playerCamera = Camera.main; // Get the main camera
        }

        private void LateUpdate()
        {
            LookAtPlayer(); // Make the UI face the player in LateUpdate each frame
        }

        private void LookAtPlayer()
        {
            // Calculate the direction the UI should face so it faces the camera
            Vector3 directionToFace = transform.position - playerCamera.transform.position;
            directionToFace.y = 0; // This will ensure that the UI only rotates on the X and Z axes, not the Y axis
            Quaternion targetRotation = Quaternion.LookRotation(directionToFace); // Calculate the target rotation
            transform.rotation = targetRotation; // Apply the target rotation
        }
    }
}
