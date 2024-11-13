using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    public Canvas shopCanvas;
    private bool playerInRange = false;
    private int itemCost;
    private PlayerGold playerGold;
    public ItemShop itemShop;

    private void Start() {
        playerGold = GameObject.FindGameObjectWithTag("UI Player Gold").GetComponent<PlayerGold>(); 
    }
    private void Update() {
        if(playerInRange && Input.GetKeyDown(KeyCode.E)) {
            // Give Player Item // Remove Gold based on Item Cost
            buyItem(itemCost);
            // Turn off Shop Canvas
            shopCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            if(gameObject.tag == "Item1Stand") {
                Debug.Log("Item Cost set to Item1 Stand");
                itemCost = itemShop.getItemCosts(1);
            }
            else if(gameObject.tag == "Item2Stand") {
                Debug.Log("Item Cost set to Item2 Stand");
                itemCost = itemShop.getItemCosts(2);
            }
            else if(gameObject.tag == "Item3Stand") {
                Debug.Log("Item Cost set to Item3 Stand");
                itemCost = itemShop.getItemCosts(3);
            }
        }
    }

    private void buyItem(int itemCost) {
        Debug.Log("item cost before remove gold call: " + itemCost);
        if(playerGold.getCurrentGold > itemCost) {
            playerGold.removeGold(itemCost);
        }
    }
}
