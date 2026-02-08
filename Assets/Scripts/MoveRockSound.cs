using UnityEngine;

public class MoveRockSound : MonoBehaviour
{
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private float minDistanceToCountAsMove = 0.002f;
    [SerializeField] private float stopDelay = 0.15f;

    private AudioSource audioSource;
    private Vector3 lastPosition;
    private bool playerTouching = false;
    private float stopTimer = 0f;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = moveClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        bool isActuallyMoving = distanceMoved > minDistanceToCountAsMove;

        if (playerTouching && isActuallyMoving)
        {
            stopTimer = stopDelay;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f && audioSource.isPlaying)
                audioSource.Stop();
        }

        lastPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerTouching = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerTouching = false;
    }
}
