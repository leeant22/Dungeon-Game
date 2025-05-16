using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;
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
        StartCoroutine(WaitForDeathAnimation(dieAnimation));
    }

    private IEnumerator WaitForDeathAnimation(Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Die01"))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(stateInfo.length);
        Destroy(gameObject);
    }
}
