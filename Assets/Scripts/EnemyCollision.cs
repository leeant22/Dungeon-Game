using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public SwordController sword;
    private bool hitRegistered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !sword.attackAllowed && !hitRegistered)
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                hitRegistered = true;
                sword.HitEnemy();
                health.Hit();

                Knockback kb = other.GetComponent<Knockback>();
                if (kb != null)
                {
                    kb.PlayFeedback(gameObject);
                }
            }
        }
    }

    public void ResetHit()
    {
        hitRegistered = false;
    }
}