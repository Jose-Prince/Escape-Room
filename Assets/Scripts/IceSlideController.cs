using UnityEngine;
using UnityEngine.Tilemaps;

public class IceSlideController : MonoBehaviour
{
    [SerializeField] Tilemap iceSlipTilemap;
    [SerializeField] private AudioClip slideClip;

    private AudioSource audioSource;
    private bool wasSlidingLastFrame = false;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = slideClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D
        audioSource.volume = 0.8f;
    }

    public bool isSliding(Vector3 feetWorldPos, Vector3 inputDir)
    {
        if (iceSlipTilemap == null) return false;

        Vector3Int cellPos = iceSlipTilemap.WorldToCell(feetWorldPos);
        TileBase iceTile = iceSlipTilemap.GetTile(cellPos);

        bool slidingNow = (iceTile != null && inputDir != Vector3.zero);

        HandleSlideSound(slidingNow);

        return slidingNow;
    }

    private void HandleSlideSound(bool slidingNow)
    {
        if (slidingNow && !wasSlidingLastFrame)
        {
            if (slideClip != null)
                audioSource.Play();
        }
        else if (!slidingNow && wasSlidingLastFrame)
        {
            audioSource.Stop();
        }

        wasSlidingLastFrame = slidingNow;
    }
}
