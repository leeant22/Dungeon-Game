using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour
{
    public GameObject Sword;
    public PlayerMove player;
    public bool attackAllowed = true;
    public float cooldown = 1.0f;
    public AudioClip attackSound;
    public AudioClip hitSound;
    private bool hitEnemy = false;
    private EnemyCollision enemyCollision;
    private BoxCollider sCollider;

    private void Awake()
    {
        enemyCollision = Sword.GetComponent<EnemyCollision>();
        sCollider = Sword.GetComponent<BoxCollider>();
        sCollider.enabled = false;
        player = FindObjectOfType<PlayerMove>();
        Debug.Log(player);
    }

    private void Update()
    {
        Debug.Log(player.playerIsAlive);
        if (Input.GetMouseButtonDown(0) && attackAllowed && player.playerIsAlive)
        {
            sCollider.enabled = true;
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
        sCollider.enabled = false;
        hitEnemy = false;
        if (enemyCollision != null)
        {
            enemyCollision.ResetHit();
        }
    }
}
