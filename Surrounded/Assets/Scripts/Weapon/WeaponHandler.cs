using UnityEngine;

namespace slowac_Weapons
{
    public class WeaponHandler : MonoBehaviour // Weapon Handler Class
    {
        public static WeaponHandler instance; // Singleton instance
        public Gun primaryGun;
        public Gun secondaryGun;

        private Gun currentGun; // Current gun in use
        private GameObject currentGunPrefab; // Instance of current gun prefab

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // If the singleton instance is null, set the current instance as a singleton
            }
            else if (instance != this)
            {
                Destroy(this); // If another instance already exists, destroy the current instance
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToPrimary(); // When the player presses the 1 key, switch to the primary gun
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
             SwitchToSecondary(); // When the player presses the 2 key, switch to the secondary gun
            }
        }

        public void PickUpGun(Gun gun)
        {
            if (primaryGun == null)
            {
                primaryGun = gun; // If the primary gun is null, set the picked up gun to the primary gun
                SwitchToPrimary(); // Switch to the primary gun
            }
            else if (secondaryGun == null)
            {
                secondaryGun = gun; // If the secondary gun is null, set the picked up gun to the secondary gun
                SwitchToSecondary(); // Switch to the secondary gun
            }
            else
            {
                // If there are already two guns, replace the currently equipped gun
                if (currentGun == primaryGun)
                {
                    // Instantiate the dropped gun prefab
                    Instantiate(primaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    primaryGun = gun; // Set the picked up gun as the primary gun
                    SwitchToPrimary(); // Switch to the primary gun
                }
                else
                {
                    // Instantiate the dropped gun prefab
                    Instantiate(secondaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    secondaryGun = gun; // Set the picked up gun as the secondary gun
                    SwitchToSecondary(); // Switch to the secondary gun
                }
            }
        }

        private void SwitchToPrimary()
        {
            if (primaryGun != null)
            {
                EquipGun(primaryGun); // If the primary gun is not null, equip the primary gun
            }
        }

        private void SwitchToSecondary()
        {
            if (secondaryGun != null)
            {
                EquipGun(secondaryGun); // If the secondary gun is not null, equip the secondary gun
            }
        }

        private void EquipGun(Gun gun)
        {
            currentGun = gun; // Set the current gun to the equipped gun
            if (currentGunPrefab != null)
            {
                Destroy(currentGunPrefab); // If the current gun prefab is not empty, destroy it
            }

            // Instantiate the selected gun and assign the instantiated prefab to the currentGunPrefab variable
            currentGunPrefab = Instantiate(gun.gameObject, transform);
            // Use the singleton of AmmunitionManager to call its AmmunitionUI component and update the ammunition type to the currently equipped gun
            AmmunitionManager.instance.ammunitionUI.UpdateAmmunitionType(currentGun);
        }
    }
}