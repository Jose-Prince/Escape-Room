using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpDuration = 0.25f;
    [SerializeField] private Tilemap waterTilemap;

    public bool IsJumping { get; private set; }

    public void JumpTo(Vector3 targetPos)
    {
        if (!IsJumping)
        {
            StartCoroutine(JumpRoutine(targetPos));
        }
    }

    // Update is called once per frame
    private IEnumerator JumpRoutine(Vector3 targetPos)
    {
        IsJumping = true;

        TilemapCollider2D tmc = waterTilemap.GetComponent<TilemapCollider2D>();
        if (tmc != null) tmc.enabled = false;

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
        IsJumping = false;

        if (tmc != null) tmc.enabled = true;
    }
}
