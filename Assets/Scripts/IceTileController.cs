using UnityEngine;
using UnityEngine.Tilemaps;

public class IceTileController : MonoBehaviour
{
    [SerializeField] Tilemap iceTilemap;
    [SerializeField] Tile iceTile;
    [SerializeField] Tile snowTile;
    [SerializeField] LevelManager levelManager;
    [SerializeField] AudioClip iceClip;

    private Vector3Int lastcellPos;
    private bool tileChangeLocked = false;

    public void HandlePlayerStep(Vector3 worldFeetPosition)
    {
        if (iceTilemap == null) return;

        Vector3Int cellPos = iceTilemap.WorldToCell(worldFeetPosition);

        if (cellPos == lastcellPos) return;
        lastcellPos = cellPos;

        TileBase currentTile = iceTilemap.GetTile(cellPos);

        if (currentTile == snowTile)
        {
            tileChangeLocked = true;
            return;
        }

        if (tileChangeLocked) return;

        if (currentTile == iceTile)
        {
            iceTilemap.SetTile(cellPos, snowTile);

            if (iceClip != null && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(iceClip);

            if (levelManager != null)
                levelManager.UpdateTilesChanged();
        }
    }

    public void ResetIceTiles()
    {
        if (iceTilemap == null) return;

        BoundsInt bounds = iceTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (iceTilemap.GetTile(pos) == snowTile)
                iceTilemap.SetTile(pos, iceTile);
        }

        tileChangeLocked = false;
        lastcellPos = Vector3Int.zero;
    }
}
