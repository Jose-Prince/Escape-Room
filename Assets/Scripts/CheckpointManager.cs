using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 currentCheckpoint;

    void Start()
    {
        LoadCheckpoint();
    }

    public void SetCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
        SaveCheckpoint(position);
        Debug.Log("Checkpoint guardado permanentemente en: " + position);
    }

    void SaveCheckpoint(Vector3 pos)
    {
        PlayerPrefs.SetFloat("CheckpointX", pos.x);
        PlayerPrefs.SetFloat("CheckpointY", pos.y);
        PlayerPrefs.SetFloat("CheckpointZ", pos.z);
        PlayerPrefs.Save();
    }

    void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");

            currentCheckpoint = new Vector3(x, y, z);
        }
        else
        {
            currentCheckpoint = transform.position;
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        return currentCheckpoint;
    }
}
