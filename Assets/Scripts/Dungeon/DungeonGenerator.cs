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

    public GameObject player;

    private List<GameObject> spawnedRooms = new List<GameObject>();

    public Vector3 roomOffset = new Vector3(20f, 0f, 20f);

    public float playerHeightOffset = 1.5f;

    public NavMeshSurface navMeshSurface;

    private int currentRoom = 0;

    // Start is called before the first frame update
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

        // Log first room generation
        //Debug.Log("First room generated at position: " + currentPosition);

        // Generate the middle rooms
        for (int i = 1; i < numberOfRooms; i++)
        {
            // Choose a random room prefab
            GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

            // Determine the next room's position
            currentPosition += roomOffset;

            // Instantiate the room at the current position
            GameObject newRoom = Instantiate(roomPrefab, currentPosition, Quaternion.identity);
            spawnedRooms.Add(newRoom);

            // Log each room generation
            //Debug.Log("Room " + i + " generated at position: " + currentPosition);
        }

        // Generate the last room
        currentPosition += roomOffset;
        GameObject lastRoom = Instantiate(lastRoomPrefab, currentPosition, Quaternion.identity);
        spawnedRooms.Add(lastRoom);

        // Log last room generation
        //Debug.Log("Last room generated at position: " + currentPosition);
    }

    void TeleportPlayerToFirstRoom()
    {
        if (spawnedRooms.Count > 0)
        {
            Vector3 firstRoomPosition = spawnedRooms[0].transform.position;

            // Apply height offset to the player's Y position
            Vector3 playerTargetPosition = new Vector3(firstRoomPosition.x, firstRoomPosition.y + playerHeightOffset, firstRoomPosition.z);

            // Debug log to check teleportation
            //Debug.Log("Teleporting player to: " + playerTargetPosition);

            player.transform.SetPositionAndRotation(playerTargetPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No rooms generated!");
        }
    }

    public void TeleportPlayerToNextRoom() 
    {
        if(currentRoom + 1 < spawnedRooms.Count) {
            currentRoom++;
            if(currentRoom >= 0 && currentRoom < spawnedRooms.Count) {
                Vector3 targetRoomPosition = spawnedRooms[currentRoom].transform.position;
                Vector3 playerTargetPosition;
                //Debug.Log("currentroom index: " + currentRoom);
                if(spawnedRooms[currentRoom].tag == "diagonal") {
                    playerTargetPosition = new Vector3(targetRoomPosition.x - 4, targetRoomPosition.y - 6 + playerHeightOffset, targetRoomPosition.z - 8);
                    //Debug.Log("Teleporting player to diagonal room" + playerTargetPosition);
                }
                else {
                    playerTargetPosition = new Vector3(targetRoomPosition.x, targetRoomPosition.y + playerHeightOffset, targetRoomPosition.z);
                    //Debug.Log("Teleporting player to next room" + playerTargetPosition);

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
