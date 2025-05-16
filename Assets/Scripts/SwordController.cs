using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour
{
    public GameObject Sword;
    public bool attackAllowed = true;
    public float cooldown = 1.0f;
    public AudioClip attackSound;
    public AudioClip hitSound;
    private bool hitEnemy = false;
    private EnemyCollision enemyCollision;

    private void Awake()
    {
        enemyCollision = Sword.GetComponent<EnemyCollision>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackAllowed)
        {
            attackAllowed = false;
            Animator swordAnimation = Sword.GetComponent<Animator>();
            swordAnimation.SetTrigger("Attack");
            StartCoroutine(HandleAttackSound());
            StartCoroutine(ResetCooldown());
        }
    }

    public void HitEnemy() {
        hitEnemy = true;
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(hitSound);
    }

    private IEnumerator HandleAttackSound()
    {
        yield return new WaitForSeconds(0.05f);
        if (!hitEnemy) {
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        attackAllowed = true;
        hitEnemy = false;
        if (enemyCollision != null)
        {
            enemyCollision.ResetHit();
        }
    }
}
