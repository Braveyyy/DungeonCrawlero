using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public Image[] itemList = new Image[3];
    public Sprite[] availableItemSprites;
    public TextMeshProUGUI[] shopPrices = new TextMeshProUGUI[3];
    private int[] intPrices = new int[3];
    private bool spawnedItems = false;

    private void OnTriggerEnter(Collider other) {
        if(!spawnedItems) {
            assignRandomItems();
            spawnedItems = true;
        }
    }

    private void assignRandomItems() {
        for(int i = 0; i < itemList.Length; i++) {
            int random = Random.Range(0, availableItemSprites.Length);
            int randomGoldPrice = Random.Range(1, 50);
            itemList[i].sprite = availableItemSprites[random];
            shopPrices[i].text = "PRICE: " + randomGoldPrice;
            intPrices[i] = randomGoldPrice;
        }
    }

    public int getItemCosts(int itemStandNum) {
        if (itemStandNum == 1) {
            return intPrices[0];
        }
        else if (itemStandNum == 2) {
            return intPrices[1];
        }
        else {
            return intPrices[2];
        }
    }

    public Sprite getItemSprite(int itemStandNum) {
        if (itemStandNum == 1) {
            return availableItemSprites[0];
        }
        else if (itemStandNum == 2) {
            return availableItemSprites[1];
        }
        else {
            return availableItemSprites[2];
        }
    }
}
