using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public int numberOfRooms = 10;
    public GameObject startRoomPrefab;
    public GameObject lastRoomPrefab;
    public GameObject floor10BossRoom;
    public GameObject player;
    private List<GameObject> spawnedRooms = new List<GameObject>();
    public Vector3 roomOffset = new Vector3(20f, 0f, 20f);
    public float playerHeightOffset = 1.5f;
    public NavMeshSurface navMeshSurface;
    private int currentRoom = 0;
    private GameObject roomPrefab;

    void Start()
    {
        GenerateDungeon();
        navMeshSurface.BuildNavMesh();
        // Teleport the player to the first room
        Invoke("TeleportPlayerToFirstRoom", 0.1f);
    }

    void GenerateDungeon()
    {
        Vector3 currentPosition = Vector3.zero;

        // Instantiate the first room (starting room)
        GameObject startRoom = Instantiate(startRoomPrefab, currentPosition, Quaternion.identity);
        spawnedRooms.Add(startRoom);

        // Generate middle rooms
        for (int i = 1; i < numberOfRooms; i++)
        {
            if(i == 10) // if floor is 10, spawn boss room
            {
                roomPrefab = floor10BossRoom;
            }
            else // else choose a random room prefab 
            {
                roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            }
            // Determine the next room's position
            currentPosition += roomOffset;

            // Instantiate the room at the current position
            GameObject newRoom = Instantiate(roomPrefab, currentPosition, Quaternion.identity);
            spawnedRooms.Add(newRoom);
        }

        // Generate the last room
        currentPosition += roomOffset;
        GameObject lastRoom = Instantiate(lastRoomPrefab, currentPosition, Quaternion.identity);
        spawnedRooms.Add(lastRoom);
    }

    void TeleportPlayerToFirstRoom()
    {
        if (spawnedRooms.Count > 0)
        {
            Vector3 firstRoomPosition = spawnedRooms[0].transform.position;

            // Apply height offset to the player's Y position
            Vector3 playerTargetPosition = new Vector3(firstRoomPosition.x, firstRoomPosition.y + playerHeightOffset, firstRoomPosition.z);

            player.transform.SetPositionAndRotation(playerTargetPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No rooms generated!");
        }
    }

    public void TeleportPlayerToNextRoom() 
    {
        if(currentRoom + 1 < spawnedRooms.Count) 
        {
            currentRoom++;
            if(currentRoom >= 0 && currentRoom < spawnedRooms.Count) 
            {
                Vector3 targetRoomPosition = spawnedRooms[currentRoom].transform.position;
                Vector3 playerTargetPosition;

                if(spawnedRooms[currentRoom].tag == "diagonal") // correct teleport positioning for diagonal room
                {
                    playerTargetPosition = new Vector3(targetRoomPosition.x - 4, targetRoomPosition.y - 6 + playerHeightOffset, targetRoomPosition.z - 8);
                }
                else // regular teleport position
                {
                    playerTargetPosition = new Vector3(targetRoomPosition.x, targetRoomPosition.y + playerHeightOffset, targetRoomPosition.z);

                }

                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    player.transform.position = playerTargetPosition;
                    cc.enabled = true;
                }
                else
                {
                    player.transform.SetPositionAndRotation(playerTargetPosition, Quaternion.identity);
                }
            }
        }
    }
}
