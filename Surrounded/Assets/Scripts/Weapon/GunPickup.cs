using slowac_Weapons;
using UnityEngine;

// Gun pickup class
public class GunPickup : MonoBehaviour, ILootable
{
    [SerializeField] private Gun gun; // The gun object associated with the pickup

    // When the player starts looking at the gun pickup
    public void OnStartLook()
    {
        //Debug.Log($"Started looking at {gun.gunName}!");
    }

    // When the player interacts with the gun pickup
    public void OnInteract()
    {
        //Debug.Log($"Trying to pick up {gun.gunName}!");
        WeaponHandler.instance.PickUpGun(gun);
        Destroy(gameObject); // Destroy the dropped gun object
    }

    // When the player stops looking at the gun pickup
    public void OnEndLook()
    {
        //Debug.Log($"Stopped looking at {gun.gunName}!");
    }
}