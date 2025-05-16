using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealth = maxHealth;
    }

    public void Hit()
    {
        currHealth--;
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Animator dieAnimation = GetComponent<Animator>();
        dieAnimation.SetTrigger("Kill");
    }
}
