using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveGate : MonoBehaviour
{
    [SerializeField] bool place;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (place)
            {
                SceneManager.LoadScene("Game");
                PlayerPrefs.DeleteAll();
            }
            else
            {
                SceneManager.LoadScene("End");                
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
