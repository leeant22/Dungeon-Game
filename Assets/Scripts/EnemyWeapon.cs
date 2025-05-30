using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyAI enemyAI;
    public AudioClip hitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<AudioSource>().PlayOneShot(hitSound);
            enemyAI.Hit();
        }
    }
}
