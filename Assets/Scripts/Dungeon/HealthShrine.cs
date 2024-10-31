using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShrine : MonoBehaviour
{
    private bool shrineActive = true;
    private bool nearShrine;
    private PlayerHealth playerHealth;
    public float healAmount;
    private void Start() {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void Update() {
        if(shrineActive && nearShrine && Input.GetKeyDown(KeyCode.E)) {
            playerHealth.healPlayer(healAmount);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            shrineActive = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            nearShrine = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            nearShrine = false;
        }
    }
    
}
