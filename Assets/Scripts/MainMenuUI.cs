using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private const string SAVE_KEY = "MusicTime";

    void Start()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
}
