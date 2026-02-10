using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField]private string nextSceneName;

    private const string MUSIC_TIME_KEY = "MusicTime";
    private bool sceneLoading = false;

    void Start()
    {
        float savedTime = PlayerPrefs.GetFloat(MUSIC_TIME_KEY, 0f);

        musicSource.time = savedTime;
        musicSource.Play();
    }

    void Update()
    {
        if (musicSource.isPlaying)
            PlayerPrefs.SetFloat(MUSIC_TIME_KEY, musicSource.time);

        if (!musicSource.isPlaying && !sceneLoading)
        {
            sceneLoading = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void OnApplicationQuit()
    {
        SaveMusicTime();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveMusicTime();
    }

    void SaveMusicTime()
    {
        PlayerPrefs.SetFloat(MUSIC_TIME_KEY, musicSource.time);
        PlayerPrefs.Save();
    }
}
