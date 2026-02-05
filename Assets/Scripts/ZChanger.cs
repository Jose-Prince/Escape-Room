using UnityEngine;

public class ZChanger : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 newPos;
    

    void Update()
    {
        if (player.transform.position.y > transform.position.y + 0.35)
        {
            newPos = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            newPos = new Vector3(transform.position.x, transform.position.y, 1);
        }

        transform.position = newPos;
    }
}
