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
    public float attackCooldown;
    private bool attackAllowed = true;
    private Animator animator;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
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
        if (!inRange && !inAttackRange)
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
        transform.LookAt(player);
        if (attackAllowed)
        {
            attackAllowed = false;
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Attack");
            StartCoroutine(ResetCooldwon());
        }
    }

    private IEnumerator ResetCooldwon()
    {
        yield return new WaitForSeconds(attackCooldown);
        animator.SetTrigger("Idle");
        agent.isStopped = false;
        attackAllowed = true;
    }
}