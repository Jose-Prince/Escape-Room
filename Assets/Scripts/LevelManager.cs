using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{

    [SerializeField] Tilemap blockTilemap;
    [SerializeField] IceTileController iceTileController;

    private int rocksPlaced = 0;
    private int tilesChanged = 0;

    private TilemapCollider2D tmc;
    private TilemapRenderer tmr;

    void Awake()
    {
        tmr = blockTilemap.GetComponent<TilemapRenderer>();
        tmc = blockTilemap.GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        if (rocksPlaced == 4)
        {
            tmr.enabled = false;

            tmc.enabled = false;
        }

        if (tilesChanged == 40)
        {
            tmr.enabled = false;

            tmc.enabled = false;
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
        iceTileController.ResetIceTiles();
    }

    public void ContinueTime()
    {
        Time.timeScale = 1f;
    }
}
