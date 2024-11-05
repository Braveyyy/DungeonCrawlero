using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private EnemyList enemyList;
    public int numberOfEnemies = 1;
    public bool spawningBoss = false;
    private bool hasSpawned = false;

    private void Start() {
        enemyList = GameObject.FindGameObjectWithTag("UI Enemy List").GetComponent<EnemyList>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            spawnEnemies();
            hasSpawned = true; // Prevent spawning more skeletons if desired
        }
    }

    private void spawnEnemies()
    {
        if(spawningBoss) {
            enemyList.createEnemyIcons(numberOfEnemies, true);
        }
        else {
            enemyList.createEnemyIcons(numberOfEnemies, false);
        }
        
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = getRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 getRandomSpawnPosition()
    {
        Collider collider = GetComponent<Collider>();
        Bounds bounds = collider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        // Use the center Y position of the collider for spawning
        float y = bounds.center.y; // Adjust this if necessary

        return new Vector3(x, y, z);
    }
}
