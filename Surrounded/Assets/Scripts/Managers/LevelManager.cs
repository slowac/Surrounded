using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Singleton mode, convenient for other scripts to access
    public GameObject[] enemySpawnPoints; // Enemy spawn point array
    public GameObject enemyPrefab; // Enemy prefab
    public RoundUI roundUI; // Round UI
    public GameObject player; // Player object
    public GameObject[] weaponPrefabs; // Weapon prefab array
    public GameObject[] ammoPrefabs; // Ammunition prefab array
    public GameObject[] enemyPrefabs; // Enemy prefab array

    public AudioClip helicopterAudioClip;
    public AudioClip weaponDropAudioClip;
    public AudioClip ammoDropAudioClip; // Add sound effects

    private int currentRound = 0; // Current round number
    private int enemiesLeft; // Remaining number of enemies
    private int totalEnemiesToSpawn; // Total number of enemies to be generated in this round
    private bool isRoundInProgress = false; // Is the round in progress
    private bool hasGameStarted = false; // Is the game started
    private int enemiesDefeatedCount = 0; // Count of enemies defeated
    private int enemiesToDropSupply = 10; // How many enemies drop supplies after being defeated
    private int supplyCounter = 0; // Add a new variable to track the number of enemies defeated
    private int cycleCount = 0;
    private int spawnCounter = 0; // Add a counter for spawning enemies
    private List<GameObject> allWeaponsCollected = new List<GameObject>();
    
    private void Awake()
    { 
        if (instance == null) // Singleton mode initialization
        { 
            instance = this;
        } 
        else if (instance != this) // If an instance already exists, destroy the new instance
        { 
            Destroy(gameObject);
        }
    }
    public void PlayerEnteredStart() // Player enters the start area
    { 
        if (!hasGameStarted) // If the game has not yet started
        { hasGameStarted = true; // Mark the start of the game
            BeginRound(); // Start the round
        }
    }

    private Vector3 GetRandomSpawnPositionNearPlayer(float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        Vector3 spawnPosition = player.transform.position + randomDirection;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, distance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return spawnPosition;

    }
    private void DropSupply()
    {
        Vector3 spawnPosition = GetRandomSpawnPositionNearPlayer(4);
        spawnPosition += Vector3.up * 5;

        Debug.Log("Playing helicopter sound");
        AudioSource.PlayClipAtPoint(helicopterAudioClip, spawnPosition);

        if (cycleCount == 0)// The first cycle
        {
            switch (supplyCounter)
            {
                case 0:
                    DropWeapon(2, spawnPosition);
                    break;
                case 1:
                    DropAmmo(spawnPosition);
                    break;
                case 2:
                    DropWeapon(4, spawnPosition);
                    break;
                case 3:
                    DropWeapon(3, spawnPosition);
                    break;
                case 4:
                    DropAmmo(spawnPosition);
                    break;
                case 5:
                    DropWeapon(5, spawnPosition);
                    break;
                case 6:
                    DropAmmo(spawnPosition);
                    break;
                case 7:
                    DropWeapon(6, spawnPosition);
                    break;
                case 8:
                    DropAmmo(spawnPosition);
                    break;
                case 9:
                    DropAmmo(spawnPosition);
                    break;
                case 10:
                    DropWeapon(7, spawnPosition);
                    DropAmmo(spawnPosition);
                    break;
                case 11:
                    DropAmmo(spawnPosition);
                    break;
                case 12:
                    DropAmmo(spawnPosition);
                    break;
            }
            supplyCounter = (supplyCounter + 1) % 12;

            // After the current cycle ends, the cycleCount is incremented to enter the next cycle
            if (supplyCounter == 0)
            {
                cycleCount++;
            }
        }
        else // Other rounds only generate ammo boxes
        {
            DropAmmo(spawnPosition);
        }

    }

    private void DropWeapon(int index, Vector3 spawnPosition)
    {
        GameObject weaponToDrop = weaponPrefabs[index];
        GameObject droppedWeapon = Instantiate(weaponToDrop, spawnPosition, Quaternion.identity);
        droppedWeapon.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(weaponDropAudioClip, spawnPosition, 2f));
    }

    private void DropAmmo(Vector3 spawnPosition)
    {
        GameObject ammoToDrop = GetRandomAmmoDrop();
        GameObject droppedAmmo = Instantiate(ammoToDrop, spawnPosition, Quaternion.identity);
        droppedAmmo.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(ammoDropAudioClip, spawnPosition, 2f));
    }

    private GameObject GetRandomAmmoDrop()
    {
        int randomIndex = -1;
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.7f) // 70% probability
        {
            randomIndex = 0; // Ammo type of current weapon
        }
        else if (randomValue < 0.85f) // 15% probability
        {
            randomIndex = 1; // Other type 1
        }
        else // 15% probability
        {
            randomIndex = 2; // Other type 2
        }

        return ammoPrefabs[randomIndex];
    }

    private IEnumerator PlayDelayedAudio(AudioClip audioClip, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Playing drop sound");
        AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void BeginRound() // Start a new round
    {
        if (!isRoundInProgress && hasGameStarted) // If the round is not in progress and the game has started
        {
            currentRound++; // Increase the number of rounds
            StartCoroutine(roundUI.ShowRoundText(currentRound)); // Display the number of rounds
            totalEnemiesToSpawn = 1 + (currentRound - 1) * 2; // Calculate the number of enemies generated in this round
            enemiesLeft = totalEnemiesToSpawn; // Initialize the number of remaining enemies
            StartCoroutine(SpawnEnemies()); // Generate enemies
            isRoundInProgress = true; // Mark the round in progress
        }
    }

    private IEnumerator SpawnEnemies() // Generate enemy coroutine
    {
        while (enemiesLeft > 0) // When there are still enemies that have not been generated
        {
            foreach (GameObject spawnPoint in enemySpawnPoints) // Traverse the generation point
            {
                // Update the generated enemy counter first
                spawnCounter++;

                // Select the type of enemy to spawn based on the spawn counter
                GameObject enemyPrefabToSpawn;

                if (spawnCounter % 15 == 0 && spawnCounter != 0) // Spawn a Boss for every 15 enemies
                {
                    enemyPrefabToSpawn = enemyPrefabs[3]; // Boss
                }
                else if (spawnCounter % 10 == 0 && spawnCounter != 0) // Spawn an Elite for every 10 enemies
                {
                    enemyPrefabToSpawn = enemyPrefabs[2]; // Elite
                }
                else if (spawnCounter % 5 == 0 && spawnCounter != 0) // Spawn a Minion for every 5 enemies
                {
                    enemyPrefabToSpawn = enemyPrefabs[1]; // Minion
                }
                else
                {
                    enemyPrefabToSpawn = enemyPrefabs[0]; // Normal
                }

                // Spawn the selected enemy type
                Instantiate(enemyPrefabToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);

                enemiesLeft--; // Update the number of remaining enemies

                if (enemiesLeft <= 0) break; // If all enemies are generated, jump out of the loop
            }
            yield return new WaitForSeconds(1); // Wait for 1 second
        }
        spawnCounter = 0; // Reset the spawn enemy counter at the end of the round
    }

    public void EnemyDefeated() // Enemy is defeated
    {
        totalEnemiesToSpawn--; // Update the total number of enemies in this round

        enemiesDefeatedCount++;
        if (enemiesDefeatedCount >= enemiesToDropSupply)
        {
            DropSupply();
            enemiesDefeatedCount = 0;
        }

        if (totalEnemiesToSpawn <= 0) // If all enemies are defeated
        {
            isRoundInProgress = false; // Mark the end of the round
        }
    }

    private void Update() // Update every frame
    {
        if (hasGameStarted && !isRoundInProgress) // If the game starts and is not in progress
        {
            BeginRound(); // Start a new round
        }
    }
}
