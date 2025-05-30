using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.Hit();
        }
    }
}
