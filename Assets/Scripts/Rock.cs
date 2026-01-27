using UnityEngine;
using UnityEngine.Tilemaps;

public class Rock : MonoBehaviour
{    
    private bool correctTile = false;

    [SerializeField] Tilemap dirtTilemap;
    [SerializeField] LevelManager levelManager;

    void Update()
    {
        Vector3Int cellPos = dirtTilemap.WorldToCell(transform.position);
        bool hasTile = dirtTilemap.HasTile(cellPos);

        if (hasTile && !correctTile)
        {
            correctTile = true;
            levelManager.UpdateRocks();
        }
        else if (!hasTile && correctTile)
        {
            correctTile = false;
            levelManager.MinusRocks();
        }
    }
}
