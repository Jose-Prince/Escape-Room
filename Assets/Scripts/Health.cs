using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float totalHealth = 10f;
    [SerializeField] Image healthBarFill;
    private float actualHealth;

    void Start()
    {
        actualHealth =totalHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            actualHealth -= 1;
            UpdateHealthBar();
        }     
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            actualHealth -= 1;
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = actualHealth / totalHealth;
    }
}