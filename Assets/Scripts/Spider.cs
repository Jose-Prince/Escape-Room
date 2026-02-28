using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveRadius = 8f;
    [SerializeField] float moveDelay = 3f;

    [SerializeField] float detectRadius = 3f;

    [Header("Attack")]
    [SerializeField] GameObject webPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent => GetComponent<NavMeshAgent>();
    private float moveTimer;
    private float attackTimer;

    private Animator anim =>GetComponent<Animator>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        MoveRandom();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        attackTimer -= Time.deltaTime;

        if (distance <= detectRadius)
        {
            TryShootWeb();
        }
        else
        {
            Patrol();
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.01f;
        anim.SetBool("IsMoving", isMoving);        
    }

    void Patrol()
    {
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

    void TryShootWeb()
    {
        if (attackTimer > 0) return;

        attackTimer = attackCooldown;

        Vector3 targetPosition = player.position;

        GameObject web = Instantiate(webPrefab, shootPoint.position, Quaternion.identity);

        WebProjectile projectile = web.GetComponent<WebProjectile>();
        projectile.SetTarget(targetPosition);
    }
}
