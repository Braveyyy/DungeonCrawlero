using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyList : MonoBehaviour
{
    public GameObject enemyUIPrefab;
    private List<GameObject> enemyIcons = new List<GameObject>();
    private int enemiesRemaining;

    public void createEnemyIcons(int enemyCount) {
        foreach (GameObject enemy in enemyIcons) {
            Destroy(enemy);
        }
        enemyIcons.Clear();

        enemiesRemaining = enemyCount;
        for(int i = 0; i < enemyCount; i++) {
            GameObject enemyIcon = Instantiate(enemyUIPrefab, gameObject.transform);
            enemyIcon.transform.localPosition = new Vector3(i * 50, -50, 0);
            enemyIcons.Add(enemyIcon);
        }
    }

    public void enemyKilled() {
        if(enemiesRemaining > 0) {
            enemiesRemaining--;
            Destroy(enemyIcons[enemiesRemaining]);
            enemyIcons.RemoveAt(enemiesRemaining);
        }
    }
}
