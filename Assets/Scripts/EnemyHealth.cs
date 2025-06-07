using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;
    public Material regular;
    public Material onHit;
    public GameObject goblin;
    public GameObject player;
    public bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealth = maxHealth;
        player = GameObject.FindWithTag("Player");
    }

    public void Hit()
    {
        currHealth--;
        StartCoroutine(HitColor());
        Knockback knockback = GetComponent<Knockback>();
        knockback.PlayFeedback(player);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitColor()
    {
        if (!isDead)
        {
            goblin.GetComponent<Renderer>().material = onHit;
            yield return new WaitForSeconds(0.2f);
            goblin.GetComponent<Renderer>().material = regular;
        }
    }

    private void Die()
    {
        isDead = true;
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
