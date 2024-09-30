using slowac_UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionManager : MonoBehaviour
{
    public static AmmunitionManager instance; // Create a singleton instance of the AmmunitionManager class

    public AmmunitionUI ammunitionUI;// Ammunition UI component

    private Dictionary<AmmunitionTypes, int> ammunitionCounts = new Dictionary<AmmunitionTypes, int>(); // Create a dictionary to store the number of each type of ammunition

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // If the singleton instance is empty, set the current instance as a singleton
        }
        else if (instance != this)
        {
            Destroy(this); // If another instance already exists, destroy the current instance
        }
    }

    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(AmmunitionTypes)).Length; i++)
        {
            ammunitionCounts.Add((AmmunitionTypes)i, 0); // Set the initial quantity of each ammunition to 0
        }
    }

    public void AddAmmunition(int value, AmmunitionTypes ammunitionType)
    {
        ammunitionCounts[ammunitionType] += value; // Add the specified number of ammunitions according to the ammunition type
        ammunitionUI.UpdateAmmunitionCount(ammunitionCounts[ammunitionType]);
    }

    public int GetAmmunitionCount(AmmunitionTypes ammunitionType)
    {
        return ammunitionCounts[ammunitionType];
    }

    public bool ConsumeAmmunition(AmmunitionTypes ammunitionType)
    {
        if (ammunitionCounts[ammunitionType] > 0)
        {
            ammunitionCounts[ammunitionType]--; // If there is ammunition, consume one round of ammunition
            ammunitionUI.UpdateAmmunitionCount(ammunitionCounts[ammunitionType]);
            return true; // Return consumption success
        }
        else
        {
            Debug.Log("No Ammo!"); // If there is no ammunition, output prompt information
            return false; // Return consumption failure
        }
    }
}