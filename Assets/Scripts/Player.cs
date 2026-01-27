using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    private float speed = 5f;
    private Vector3 movement;

    private float jumpDuration = 0.25f;
    private bool isJumping = false;
    public bool isSliding = false;

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
    } 
    // Update is called once per frame
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

        if (isJumping) return;

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
            // Detect two tiles ahead
            Vector3Int dir = Vector3Int.zero;

            if (Input.GetKeyDown(KeyCode.W)) dir = Vector3Int.up;
            else if (Input.GetKeyDown(KeyCode.S)) dir = Vector3Int.down;
            else if (Input.GetKeyDown(KeyCode.A)) dir = Vector3Int.left;
            else if (Input.GetKeyDown(KeyCode.D)) dir = Vector3Int.right;

            if (dir == Vector3Int.zero) return;

            Vector3Int middleCell = playerGroundCellPosition + dir;
            Vector3Int targetCell = playerGroundCellPosition + dir * 2;

            TileBase waterTile = waterTilemap.GetTile(middleCell);
            TileBase platformTile = platformTilemap.GetTile(targetCell);
            TileBase groundTile = groundTilemap.GetTile(targetCell);

            if (waterTile != null && (platformTile != null || groundTile))
            {
                Vector3 targetPos = groundTilemap.GetCellCenterWorld(targetCell);
                targetPos.z = 0;
                StartCoroutine(JumpTo(targetPos));
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

    void FixedUpdate()
    {
        if (isJumping) return;

        transform.position += movement * speed * Time.deltaTime;     
    }

    IEnumerator JumpTo(Vector3 targetPos)
    {
        isJumping = true;

        TilemapCollider2D tmc = waterTilemap.GetComponent<TilemapCollider2D>();
        tmc.enabled = false;

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isJumping = false;
        tmc.enabled = true;
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
