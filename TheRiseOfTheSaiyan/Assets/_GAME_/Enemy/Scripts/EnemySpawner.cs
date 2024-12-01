using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  
    [SerializeField] private float minimumSpawnTime = 2f;  
    [SerializeField] private float maximumSpawnTime = 5f;  

    [SerializeField] private int maxEnemies = 10;  
    [SerializeField] private bool isLimitless = false;  

    private float timeUntilSpawn;  
    private int currentEnemyCount = 0; 

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
        if (!isLimitless && currentEnemyCount >= maxEnemies)
        {
            return;  
        }

        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        currentEnemyCount++;

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogWarning("Spawned enemy does not have an Enemy script attached.");
            return;
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = UnityEngine.Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
    public void SetLimitless(bool limitless)
    {
        isLimitless = limitless;
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }
}
