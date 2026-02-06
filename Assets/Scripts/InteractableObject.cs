using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject instructionsDisplay;
    [SerializeField] string message;
    private bool playerInRange = false;
    private GameObject canvas;

    void Start()
    {
        Canvas c = GetComponentInChildren<Canvas>(true);

        if (c != null)
        {
            canvas = c.gameObject;
        }

        TextMeshProUGUI instructionMsg = instructionsDisplay.GetComponentInChildren<TextMeshProUGUI>(true);
        instructionMsg.text = message;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerInRange = true;
            canvas.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerInRange = false;
            canvas.SetActive(false);
            instructionsDisplay.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();    
        }   
    }

    public void Interact()
    {
        instructionsDisplay.SetActive(true);
    }
}
