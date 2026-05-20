using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public int health;
    public int score;

    [SerializeField] private UnityEvent onZeroHealth;
    public UnityEvent onDamage;
    public UnityEvent onDestruction;

    [Header("Broadcasting Events")]
    public IntGameEvent scorer;

    public void Damage(int damageAmount)
    { 
        health -= damageAmount;
        Debug.LogFormat("Current Health = {0}", health);
        onDamage?.Invoke();
        scorer?.Raise(score);
        if (health <= 0)
        {
            OnZeroHealth();
            onDamage = null;
            onDestruction?.Invoke();

        }
    }

    public void SetHealth(int value)
    { 
        health = value;
    }

    public void OnZeroHealth()
    { 
        onZeroHealth?.Invoke();
    }
}
