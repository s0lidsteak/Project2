using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int MaxHealth = 100;
    int currentHealth;

    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onDeath.Invoke();
        }
        else
        {
            onDamaged.Invoke();
        }
    }
}