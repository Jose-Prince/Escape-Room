using UnityEngine;
using UnityEngine.Tilemaps;

public class IceSlideController : MonoBehaviour
{
    [SerializeField] Tilemap iceSlipTilemap;

    public bool isSliding(Vector3 feetWorldPos, Vector3 inputDir)
    {
        Vector3Int cellPos = iceSlipTilemap.WorldToCell(feetWorldPos);
        TileBase iceTile = iceSlipTilemap.GetTile(cellPos);

        if (iceTile != null && inputDir != Vector3.zero)
            return true;

        return false;
    }
}
