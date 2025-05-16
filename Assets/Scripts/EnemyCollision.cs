using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public SwordController sword;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !sword.attackAllowed) {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null) { 
                sword.HitEnemy();
                health.Hit();
            }
        }
    } 
}
