using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    private float speed = 5f;
    private Vector3 movement;

    public bool isSliding = false;
    private PlayerJump playerJump;

    private Vector3Int lastCellPos;
    private bool tileChangeLocked = false;

    private Collider2D col;

    [SerializeField] LevelManager levelManager;

    [Header("Tilemaps")]
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap waterTilemap;
    [SerializeField] Tilemap platformTilemap;
    [SerializeField] Tilemap iceTilemap;
    [SerializeField] Tile iceTile;
    [SerializeField] Tile snowTile;
    [SerializeField] Tilemap iceSlipTilemap;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        playerJump = GetComponent<PlayerJump>();
    } 

    void Update()
    {
        Vector3 feetPosition = new Vector3(
            col.bounds.center.x,
            col.bounds.min.y + 0.05f,
            0
        );
        
        Vector3Int playerGroundCellPosition = groundTilemap.WorldToCell(feetPosition);
        TileBase groundUnderPlayer = groundTilemap.GetTile(playerGroundCellPosition);
        
        if (groundUnderPlayer != null) isSliding = false;

        if (playerJump.IsJumping) return;
        if (isSliding) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement = new Vector3(x, y, 0).normalized;
        
        Vector3Int playerIceCellPosition = iceSlipTilemap.WorldToCell(feetPosition);
        TileBase iceUnderPlayer = iceSlipTilemap.GetTile(playerIceCellPosition);


        if (iceUnderPlayer != null && (x != 0 || y != 0))
        {
            isSliding = true;
        } 
        else
        {
            isSliding = false;    
        }
        
        Vector3Int playerPlatformCellPosition = platformTilemap.WorldToCell(transform.position);
        TileBase platformUnderPlayer = platformTilemap.GetTile(playerPlatformCellPosition);

        if (groundUnderPlayer != null || platformUnderPlayer != null)
        {
            Vector3Int dir = GetDirectionInput();
            if (dir != Vector3Int.zero)
            {
                CheckAndJumpTile(playerGroundCellPosition, dir);
            } 
        }

        Vector3Int cellPos = iceTilemap.WorldToCell(feetPosition);

        if (cellPos == lastCellPos) return;
        lastCellPos = cellPos;

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
            levelManager.UpdateTilesChanged();
        }
    }

    private Vector3Int GetDirectionInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector3Int.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector3Int.down;
        if (Input.GetKeyDown(KeyCode.A)) return Vector3Int.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector3Int.right;
        return Vector3Int.zero;
    }

    private void CheckAndJumpTile(Vector3Int currentCell, Vector3Int dir)
    {
        Vector3Int middleCell = currentCell + dir;
        Vector3Int targetCell = currentCell + dir * 2;

        TileBase waterTile = waterTilemap.GetTile(middleCell);
        TileBase platformTile = platformTilemap.GetTile(targetCell);
        TileBase groundTile = groundTilemap.GetTile(targetCell);

        if (waterTile != null && (platformTile != null || groundTile != null))
        {
            Vector3 targetPos = groundTilemap.GetCellCenterWorld(targetCell);
            targetPos.z = 0;
            playerJump.JumpTo(targetPos);
        }
    }

    void FixedUpdate()
    {
        if (playerJump.IsJumping) return;

        transform.position += movement * speed * Time.deltaTime;     
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = new Vector3(-1, -2, 0);
        }     

        if (collision.gameObject.CompareTag("Wall"))
        {
            isSliding = false;
            SnapToTileCenter(iceSlipTilemap);
        }
    }

    public void ResetIceTiles()
    {
        BoundsInt bounds = iceTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = iceTilemap.GetTile(pos);

            if (tile == snowTile)
            {
                iceTilemap.SetTile(pos, iceTile);
            }
        }

        tileChangeLocked = false;
        lastCellPos = Vector3Int.zero;
    }

    void SnapToTileCenter(Tilemap tilemap)
    {
        Vector3Int cellPos = tilemap.WorldToCell(transform.position);
        Vector3 centerPos = tilemap.GetCellCenterWorld(cellPos);
        centerPos.z = transform.position.z;
        centerPos.y -= 0.2f;
        transform.position = centerPos;
    }
}
