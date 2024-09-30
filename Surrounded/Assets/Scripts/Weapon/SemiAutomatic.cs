﻿using UnityEngine;

// Semi-automatic gun class
public class SemiAutomatic : Gun
{
    // Check if the left mouse button is pressed in Update
    private void Update()
    {
        // If the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // If the time exceeds the time of the last shot plus the firing rate
            if (Time.time - timeOfLastShot >= 1 / fireRate)
            {
                // Fire the bullet
                Fire();
                // Update the time of the last shot
                timeOfLastShot = Time.time;
            }
        }
    }
}