using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int health = 90;
    public Scrollbar healthBar;
    public GameObject healthHandle;
    public PlayerMove playerMove;

    public void Reset()
    {
        health = 90;
        healthHandle.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.size = health/100.0f;

        if (health <= -10)
        {
            healthHandle.SetActive(false);
            playerMove.PlayerDead();
        }
    }

    public void TakeDamage()
    {
        TakeDamage(10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            TakeDamage(health+10);
            // playerIsAlive = false;
            // health = 0;
            // healthText.text = "Health: " + health.ToString();
            playerMove.PlayerDead();
        } 
    }
}
