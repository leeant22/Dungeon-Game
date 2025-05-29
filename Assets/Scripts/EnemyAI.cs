using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private EnemyHealth health;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGound;
    public LayerMask whatIsPlayer;
    public float detectRange;
    public float attackRange;
    public bool inRange;
    public bool inAttackRange;
    public Vector3 walkPoint;
    private bool pointSet;
    public float walkRange;
    public float attackCooldown = 2.5f;
    public float attackTime = 1f;
    private bool attackAllowed = true;
    private Animator animator;
    private BoxCollider clubCollider;
    public PlayerHealth playerHealth;
    private bool isSwinging = false;
    private GameObject goblin;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        clubCollider = transform.Find("deb_Goblin01_W").GetComponent<BoxCollider>();
        clubCollider.enabled = false;
        goblin = transform.Find("deb_Goblin01_W").gameObject;
    }

    private void Update()
    {
        inRange = Physics.CheckSphere(transform.position, detectRange, whatIsPlayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        // float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // inAttackRange = distanceToPlayer <= attackRange;

        if (health.currHealth <= 0)
        {
            agent.isStopped = true;
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Kill");
            return;
        }

        if ((!inRange && !inAttackRange) || !attackAllowed)
        {
            // Wander();
            animator.SetTrigger("Idle");
            agent.SetDestination(transform.position);
        }
        else if (inRange && !inAttackRange)
        {
            Follow();
        }
        else if (inAttackRange)
        {
            Attack();
        }
       
    }

    // private void Wander()
    // {
    //     if (!pointSet)
    //     {
    //         animator.SetTrigger("Idle");
    //         walkPoint = new Vector3(transform.position.x + Random.Range(-walkRange, walkRange),
    //                                 transform.position.y,
    //                                 transform.position.z + Random.Range(-walkRange, walkRange));
    //         if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGound))
    //         {
    //             pointSet = true;
    //         }
    //     }

    //     if (pointSet)
    //     {
    //         agent.SetDestination(walkPoint);
    //         animator.SetTrigger("Walk");
    //     }
    //     if ((transform.position - walkPoint).magnitude < 1f)
    //     {
    //         pointSet = false;
    //     }
    // }

    private void Follow()
    {
        agent.SetDestination(player.position);
        animator.SetTrigger("Walk");
    }

    private void Attack()
    {
        agent.isStopped = true;
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        if (attackAllowed)
        {
            clubCollider.enabled = true;
            attackAllowed = false;
            isSwinging = true;
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Attack");
            StartCoroutine(ResetAttackTime());
            StartCoroutine(ResetCooldown());
        }
    }

    public void Hit() {
        if (isSwinging) 
        {
            playerHealth.TakeDamage(goblin);
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        animator.SetTrigger("Idle");
        agent.isStopped = false;
        attackAllowed = true;
    }

    private IEnumerator ResetAttackTime()
    {
        yield return new WaitForSeconds(attackTime);
        clubCollider.enabled = false;
        isSwinging = false;
    }
}