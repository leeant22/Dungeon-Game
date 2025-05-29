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

    public void TakeDamage(int damage, GameObject sender)
    {
        health -= damage;
        healthBar.size = health/100.0f;

        Knockback knockback = GetComponent<Knockback>();
        if (knockback != null && sender != null)
        {
            knockback.PlayFeedback(sender);
        }

        if (health <= -10)
        {
            healthHandle.SetActive(false);
            playerMove.PlayerDead();
        }
    }

    public void TakeDamage(GameObject sender)
    {
        TakeDamage(10, sender);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            TakeDamage(health+10, null);
            // playerIsAlive = false;
            // health = 0;
            // healthText.text = "Health: " + health.ToString();
            playerMove.PlayerDead();
        } 
    }
}
