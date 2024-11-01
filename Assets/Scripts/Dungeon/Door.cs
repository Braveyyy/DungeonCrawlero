using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private DungeonGenerator dg;
    public static bool doorActive = true;
    private bool playerInRange = false;

    private void Start() {
        dg = GameObject.FindGameObjectWithTag("Dungeon Generator").GetComponent<DungeonGenerator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("player in DOOR collider");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("player left DOOR collider");
        }
    }

    void Update() {
        if(GameObject.FindGameObjectWithTag("Skeleton") != null) {
            doorActive = false;
        } 
        else {
            doorActive = true;
        }
        if(playerInRange && doorActive && Input.GetKeyDown(KeyCode.E)) {
            dg.TeleportPlayerToNextRoom();
            playerInRange = false;
        }
    }


}
