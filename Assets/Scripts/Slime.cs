using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveRadius = 8f;
    [SerializeField] float moveDelay = 3f;

    [SerializeField] float detectRadius = 3f;

    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float normalRadius = 0.5f;
    [SerializeField] float attackCooldown = 1.5f;

    private Transform player;
    private NavMeshAgent agent;

    private float moveTimer;
    private bool attacking = false;
    private float attackTimer;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        attackCollider.radius = normalRadius;

        MoveRandom();
    }

    void Update()
{
    float distance = Vector2.Distance(transform.position, player.position);

    if (distance <= detectRadius)
    {
        Attack();
    }
    else
    {
        StopAttack();
        Patrol();
    }

    UpdateAnimation();
}

    void Patrol()
    {
        if (attacking) return;

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDelay)
        {
            MoveRandom();
            moveTimer = 0f;
        }
    }

    void MoveRandom()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            attackTimer = attackCooldown;

            attackCollider.radius = attackRadius;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            attacking = false;
            attackCollider.radius = normalRadius;
        }
    }

    void StopAttack()
    {
        if (!attacking) return;

        attacking = false;
        attackCollider.radius = normalRadius;
    }

    void UpdateAnimation()
    {
        anim.SetBool("IsAttacking", attacking);

        bool isMoving = agent.velocity.sqrMagnitude > 0.01f && !attacking;
        anim.SetBool("IsMoving", isMoving);
    }
}