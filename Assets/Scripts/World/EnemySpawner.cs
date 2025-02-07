using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public LayerMask whatIsGround; // Layer mask to determine what is ground
    public float minSpawnDelay = 5f; // Minimum delay between spawns
    public float maxSpawnDelay = 10f; // Maximum delay between spawns
    public int maxEnemiesOnScreen = 5; // Maximum number of enemies allowed on screen at once

    public bool spawnForever = true; // Whether the spawner should keep spawning enemies indefinitely
    public int totalEnemiesToSpawn = 10; // Number of enemies to spawn if spawnForever is false

    private int currentEnemiesOnScreen = 0; // Current number of enemies on screen
    private int totalEnemiesSpawned = 0; // Total number of enemies spawned

    void Start()
    {
        if (spawnForever)
        {
            // Start spawning enemies indefinitely
            InvokeRepeating("SpawnEnemy", 0f, Random.Range(minSpawnDelay, maxSpawnDelay));
        }
        else
        {
            // Start spawning enemies until totalEnemiesToSpawn is reached
            InvokeRepeating("SpawnEnemy", 0f, Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    void SpawnEnemy()
    {
        if ((spawnForever || totalEnemiesSpawned < totalEnemiesToSpawn) && currentEnemiesOnScreen < maxEnemiesOnScreen)
        {
            // Calculate a random point within a 10f radius from the spawn point
            Vector3 spawnPoint = RandomPointOnGround(transform.position, 10f);

            // Pick a random enemy from the array
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // Spawn the enemy at the random point
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

            currentEnemiesOnScreen++;
            totalEnemiesSpawned++;

            if (!spawnForever && totalEnemiesSpawned >= totalEnemiesToSpawn)
            {
                // Stop spawning if totalEnemiesToSpawn is reached
                CancelInvoke("SpawnEnemy");
            }
        }
    }

    Vector3 RandomPointOnGround(Vector3 center, float range)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        randomPoint.y = 0f; // Ensure that the point is at ground level

        // Check if the random point is on the ground
        RaycastHit hit;
        if (Physics.Raycast(randomPoint + Vector3.up * 10f, Vector3.down, out hit, 20f, whatIsGround))
        {
            randomPoint.y = hit.point.y; // Set the y coordinate to the ground level
            return randomPoint;
        }
        else
        {
            // If raycast doesn't hit the ground, return the original center
            return center;
        }
    }

    // Method to decrease the count of enemies on screen
    public void DecreaseEnemyCount()
    {
        currentEnemiesOnScreen--;
    }
}
