using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    private EnemyList enemyList;
    public int numberOfSkeletons = 1;
    private bool hasSpawned = false;

    private void Start() {
        enemyList = GameObject.FindGameObjectWithTag("UI Enemy List").GetComponent<EnemyList>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the collider
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnSkeletons();
            hasSpawned = true; // Prevent spawning more skeletons if desired
        }
    }

    private void SpawnSkeletons()
    {
        enemyList.createEnemyIcons(numberOfSkeletons);
        for (int i = 0; i < numberOfSkeletons; i++)
        {
            // Spawn the skeleton at a random position within the room
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log("Skeleton(s) spawned!");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the room (adjust based on your collider size)
        Collider collider = GetComponent<Collider>();
        Bounds bounds = collider.bounds;

        // Generate a random position within the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        // Use the center Y position of the collider for spawning
        float y = bounds.center.y; // Adjust this if necessary

        return new Vector3(x, y, z);
    }
}
