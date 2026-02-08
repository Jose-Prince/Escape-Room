using UnityEngine;

public class Bat : MonoBehaviour
{
    private Vector3 movement;

    [SerializeField] int dir = 1;
    [SerializeField] float speed = 5f;

    [Header("Sound")]
    [SerializeField] private AudioClip batSound;
    [SerializeField] private float soundRadius = 5f;

    private Transform player;
    private AudioSource audioSource;
    private bool playerInRange = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = batSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Sonido 3D
        audioSource.minDistance = 1f;
        audioSource.maxDistance = soundRadius;
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        movement = new Vector3(0, dir, 0).normalized;
        CheckPlayerDistance();
    }

    void FixedUpdate()
    {
        transform.position += movement * speed * Time.deltaTime;
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= soundRadius)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                audioSource.Play();
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                audioSource.Stop();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            dir *= -1;
    }
}
