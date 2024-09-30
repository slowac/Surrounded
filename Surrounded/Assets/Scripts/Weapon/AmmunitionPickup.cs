using UnityEngine;

// Ammunition pickup class
public class AmmunitionPickup : MonoBehaviour, ILootable
{
    [SerializeField] private int ammunitionCount; // Ammunition quantity
    [SerializeField] private AmmunitionTypes ammunitionType; // Ammunition type

    // When the player starts looking at ammunition
    public void OnStartLook()
    {
        // Show tooltip UI
        //Debug.Log($"Started looking at {ammunitionType}!");
    }

    // When the player interacts with ammunition
    public void OnInteract()
    {
        // Add ammunition
        AmmunitionManager.instance.AddAmmunition(ammunitionCount, ammunitionType);
        // Destroy the ammunition pickup object
        Destroy(gameObject);
    }

    // When the player stops looking at ammunition
    public void OnEndLook()
    {
        // Hide tooltip UI
        //Debug.Log($"Stopped looking at {ammunitionType}!");
    }
}
