using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;
    public Material regular;
    public Material onHit;
    public GameObject goblin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealth = maxHealth;
    }

    public void Hit()
    {
        currHealth--;
        StartCoroutine(HitColor());

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitColor()
    {
        goblin.GetComponent<Renderer>().material = onHit;
        yield return new WaitForSeconds(0.2f);
        goblin.GetComponent<Renderer>().material = regular;
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
