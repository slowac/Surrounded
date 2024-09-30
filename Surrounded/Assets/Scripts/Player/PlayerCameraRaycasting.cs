using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRaycasting : MonoBehaviour
{
    [SerializeField] private float raycastDistance; // Ray distance

    private ILootable currentTarget; // Current target

    private void Update()
    {
        HandleRaycast(); // Handle ray detection

        if (Input.GetKeyDown(KeyCode.E)) // If the E key is pressed
        {
            if (currentTarget != null) // If there is a target
            {
                currentTarget.OnInteract(); // Interact with the current target
            }
        }
    }

    private void HandleRaycast()
    {
        RaycastHit whatIHit; // Result of ray collision

        if (Physics.Raycast(transform.position,
        transform.forward,
        out whatIHit, raycastDistance)) // Send a ray forward from the current position
        {
            ILootable lootable = whatIHit.collider.GetComponent<ILootable>(); // Get the ILootable interface on the collision object

            if (lootable != null) // If the ILootable interface exists
            {
                if (lootable == currentTarget) // If the collision is the current target
                {
                    return;
                }
                else if (currentTarget != null) // If the current target is not null
                {
                    currentTarget.OnEndLook(); // End observation of the current target
                    currentTarget = lootable; // Set the new target as the current target
                    currentTarget.OnStartLook(); // Start observing the new target
                }
                else
                {
                    currentTarget = lootable; // Set the new target as the current target
                    currentTarget.OnStartLook(); // Start observing the new target
                }
            }
            else
            {
                if (currentTarget != null) // If the current target is not null
                {
                    currentTarget.OnEndLook(); // End observation of the current target
                    currentTarget = null; // Set the current target to null
                }
            }
        }
        else // If the ray does not detect an object
        {
            if (currentTarget != null) // If the current target is not null
            {
                currentTarget.OnEndLook(); // End observation of the current target
                currentTarget = null; // Set the current target to null
            }
        }
    }
}