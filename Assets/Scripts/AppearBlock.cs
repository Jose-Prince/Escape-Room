using UnityEngine;
using UnityEngine.Tilemaps;

public class AppearBlock : MonoBehaviour
{
    [SerializeField] private Tilemap blockTilemap; // Puede ser null
    private TilemapRenderer tmr;

    void Awake()
    {
        if (blockTilemap != null)
        {
            tmr = blockTilemap.GetComponent<TilemapRenderer>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (tmr != null)
        {
            tmr.enabled = true;
        }
    }
}
