using System;
using UnityEngine;

public class StandardHealth : MonoBehaviour, IHealth {
    [SerializeField] private int startingHealth = 100;

    private int currentHealth;

    public event Action<float> OnHPPctChanged = delegate { };
    public event Action OnDied = delegate { };
    private Animation damageEffect;


    private void Start() {
        currentHealth = startingHealth;
        damageEffect = GameObject.Find("Damage effect").GetComponent<Animation>();

    }

    public float CurrentHpPct {
        get { return (float) currentHealth / (float) startingHealth; }
    }

    public void TakeDamage(int amount) {
        if (amount <= 0) {
            throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);
        }

        currentHealth -= amount;
        damageEffect.Play("Damage Effect");


        OnHPPctChanged(CurrentHpPct);

        if (CurrentHpPct <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }
}