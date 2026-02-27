using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Bat : MonoBehaviour
{
    private Vector3 movement;

    [Header("Speeds")]
    [SerializeField] float patrolSpeed = 5f;
    [SerializeField] float dashSpeed = 10f;

    [Header("Sound")]
    [SerializeField] AudioClip batSound;

    [Header("Detection")]
    [SerializeField] float detectRadius = 2f;
    [SerializeField] float dashDistance = 4f;

    [Header("Waypoints")]
    [SerializeField] Transform Point1;
    [SerializeField] Transform Point2;

    private Transform playerT;
    private AudioSource audioSource;

    private NavMeshAgent agent;
    private Transform currentTarget;

    private bool isDashing = false;

    private Vector3 dashDirection;
    private float dashTravelled;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = batSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = detectRadius;
    }

    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentTarget = Point1;
        agent.speed = patrolSpeed;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, playerT.position);

        if (!isDashing && distance <= detectRadius)
        {
            StartDash();
        }

        if (isDashing)
            Dash();
        else
            Patrol();
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentTarget = currentTarget == Point1 ? Point2 : Point1;
            agent.SetDestination(currentTarget.position);
        }
    }

    void StartDash()
    {
        isDashing = true;
        agent.enabled = false;
        dashDirection = (playerT.position - transform.position).normalized;
        dashTravelled = 0f;

        audioSource.Play();
    }

    void Dash()
    {
        float step = dashSpeed * Time.deltaTime;
        transform.position += dashDirection * step;
        dashTravelled += step;

        if (dashTravelled >= dashDistance)
            StopDash();
    }

    void StopDash()
    {
        float distance = Vector2.Distance(transform.position, playerT.position);

        if (distance <= detectRadius)
            StartDash();
        else
        {
            isDashing = false;

            agent.enabled = true;
            agent.speed = patrolSpeed;
            agent.SetDestination(currentTarget.position);
        }
    }
}
