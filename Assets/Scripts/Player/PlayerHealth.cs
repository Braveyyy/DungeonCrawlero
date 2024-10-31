using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Animator animator; // make sure animator is disabled in inspector

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to avoid going below 0

        if (currentHealth <= 0)
        {
            //animator.SetTrigger("playerDeath");
            animator.enabled = true;
            Invoke(nameof(playerDie), 1.8f);
        }
    }

    public void healPlayer(float heal) {
        if((currentHealth + heal) > maxHealth) {
            currentHealth += maxHealth - currentHealth;
        }
        else {
            currentHealth += heal;
        }
    }

    private void playerDie()
    {
        //GameObject.FindGameObjectWithTag("Player").SetActive(false);
        // Implement death logic here (e.g., restart level, show game over screen)
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
