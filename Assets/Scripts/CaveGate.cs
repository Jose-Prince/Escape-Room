using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveGate : MonoBehaviour
{
    [SerializeField] bool place;
    [SerializeField] AudioClip triggerSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (place)
            {
                AudioManager.Instance.PlaySFX(triggerSound);
                SceneManager.LoadScene("Game");
            }
            else
            {
                SceneManager.LoadScene("End");                
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
