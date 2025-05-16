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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackAllowed) {
            attackAllowed = false;
            Animator swordAnimation = Sword.GetComponent<Animator>();
            swordAnimation.SetTrigger("Attack");
            StartCoroutine(HandleAttackSound());
            StartCoroutine(ResetCooldwon());
        }
    }

    public void HitEnemy() {
        hitEnemy = true;
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(hitSound);
    }

    private IEnumerator HandleAttackSound()
    {
        yield return null;
        if (!hitEnemy) {
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }
    IEnumerator ResetCooldwon() {
        yield return new WaitForSeconds(cooldown);
        attackAllowed = true;
        hitEnemy = false;
    }
}
