using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace slowac_UI
{
    public class AmmunitionUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammunitionTypeText; // Text that displays the current gun type
        [SerializeField] TextMeshProUGUI ammunitionCountText; // Text that displays the current ammunition count

        private CanvasGroup canvasGroup; // CanvasGroup component of the UI element

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>(); // Get the CanvasGroup component
        }

        // Update the displayed ammunition type and quantity
        public void UpdateAmmunitionType(Gun gun)
        {
            if (gun == null)
            {
                canvasGroup.alpha = 0; // If there is no gun, set the UI transparency to 0 (hide)
                return;
            }

            canvasGroup.alpha = 1; // If there is a gun, set the UI transparency to 1 (show)

            // Update the displayed ammunition count
            UpdateAmmunitionCount(AmmunitionManager.instance.GetAmmunitionCount(gun.ammunitionType));

            // Update the displayed ammunition type text
            ammunitionTypeText.text = gun.ammunitionType.ToString();
        }

        // Update the displayed ammunition count text
        public void UpdateAmmunitionCount(int newCount)
        {
            ammunitionCountText.text = newCount.ToString();
        }
    }
}

