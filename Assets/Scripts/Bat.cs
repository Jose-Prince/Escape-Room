using UnityEngine;

public class Bat : MonoBehaviour
{
    private Vector3 movement;

    [SerializeField] int dir = 1;
    [SerializeField] float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(dir, 0, 0).normalized;
    }

    void FixedUpdate()
    {
        transform.position += movement * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            dir *= -1;
        }
    }
}
