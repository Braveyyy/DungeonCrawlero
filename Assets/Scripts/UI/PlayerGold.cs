using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    private int currentPlayerGold;
    private TextMeshProUGUI goldUI;
    private void Awake() {
        currentPlayerGold = 0;
        goldUI = GetComponent<TextMeshProUGUI>();
        goldUI.text = "GOLD: " + currentPlayerGold;
    }

    public void addGold(int goldAmount) {
        currentPlayerGold += goldAmount;
        goldUI.text = "GOLD: " + currentPlayerGold;
    }
    public void removeGold(int goldAmount) {
        currentPlayerGold -= goldAmount;
        goldUI.text = "GOLD: " + currentPlayerGold;
    }
    public int getCurrentGold() {
        return currentPlayerGold;
    }
}
