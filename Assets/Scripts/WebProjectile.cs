using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    [SerializeField] float speed = 6f;

    private Vector3 target;

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }

    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
}