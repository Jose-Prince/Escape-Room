using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AppearBlock : MonoBehaviour
{
    [SerializeField] Tilemap blockTilemap;
    private TilemapRenderer tmr;

    void Awake()
    {
        tmr = blockTilemap.GetComponent<TilemapRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        tmr.enabled = true;
    }
}
