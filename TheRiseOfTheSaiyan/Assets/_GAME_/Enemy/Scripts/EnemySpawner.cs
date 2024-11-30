using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SpawnEnemy()
    {
        // Instantiate the enemy at the spawner's position
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Ensure the enemy has a health bar prefab attached
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogWarning("Spawned enemy does not have an Enemy script attached.");
            return;
        }

        // Optionally set additional properties on the spawned enemy (if needed)
        Debug.Log("Enemy spawned successfully.");
    }

    private void SetTimeUntilSpawn()
    {
        // Specify UnityEngine.Random to avoid ambiguity
        timeUntilSpawn = UnityEngine.Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
