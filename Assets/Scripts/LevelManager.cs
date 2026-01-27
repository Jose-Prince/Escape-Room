using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{

    [SerializeField] Tilemap blockTilemap;
    [SerializeField] Tilemap block2Tilemap;
    [SerializeField] Player player;

    private int rocksPlaced = 0;
    private int tilesChanged = 0;

    private TilemapCollider2D tmc;
    private TilemapRenderer tmr;
    private TilemapCollider2D tmc2;
    private TilemapRenderer tmr2;

    void Awake()
    {
        tmr = blockTilemap.GetComponent<TilemapRenderer>();
        tmc = blockTilemap.GetComponent<TilemapCollider2D>();

        tmr2 = block2Tilemap.GetComponent<TilemapRenderer>();
        tmc2 = block2Tilemap.GetComponent<TilemapCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rocksPlaced == 4)
        {
            tmr.enabled = false;

            tmc.enabled = false;
        }

        if (tilesChanged == 40)
        {
            tmr2.enabled = false;

            tmc2.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTilesPuzzle();
        }
    }

    public void UpdateRocks()
    {
        rocksPlaced++;
    }

    public void MinusRocks()
    {
        if (rocksPlaced > 0)
        {
            rocksPlaced--;
        } 
        else
        {
            rocksPlaced = 0;
        }
    }

    public void UpdateTilesChanged()
    {
        tilesChanged++;
    }

    void ResetTilesPuzzle()
    {
        tilesChanged = 0;
        player.ResetIceTiles();
    }
}
