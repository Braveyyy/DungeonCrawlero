using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseItem : MonoBehaviour
{
    public Canvas shopCanvas;
    private bool playerInRange = false;
    private int itemCost;
    private Image itemSlot;
    private int itemStand;
    private PlayerGold playerGold;
    public ItemShop itemShop;

    private void Start() {
        playerGold = GameObject.FindGameObjectWithTag("UI Player Gold").GetComponent<PlayerGold>(); 
        itemSlot = GameObject.FindGameObjectWithTag("ItemSlot").GetComponent<Image>();
    }
    private void Update() {
        if(playerInRange && Input.GetKeyDown(KeyCode.E)) {
            // Give Player Item // Remove Gold based on Item Cost
            buyItem(itemCost);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            if(gameObject.tag == "Item1Stand") {
                Debug.Log("Item Cost set to Item1 Stand");
                itemCost = itemShop.getItemCosts(1);
                itemStand = 1;
            }
            else if(gameObject.tag == "Item2Stand") {
                Debug.Log("Item Cost set to Item2 Stand");
                itemCost = itemShop.getItemCosts(2);
                itemStand = 2;
            }
            else if(gameObject.tag == "Item3Stand") {
                Debug.Log("Item Cost set to Item3 Stand");
                itemCost = itemShop.getItemCosts(3);
                itemStand = 3;
            }
        }
    }

    private void buyItem(int itemCost) {
        Debug.Log("item cost before remove gold call: " + itemCost);
        if(playerGold.getCurrentGold() > itemCost) {
            playerGold.removeGold(itemCost);
            itemSlot.sprite = itemShop.getItemSprite(itemStand);
            shopCanvas.gameObject.SetActive(false); // Turn off Shop Canvas
        }
    }
}
