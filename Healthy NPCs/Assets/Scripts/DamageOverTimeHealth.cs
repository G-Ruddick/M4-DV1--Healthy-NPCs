using System;
using System.Collections;
using UnityEngine;

public class DamageOverTimeHealth : MonoBehaviour, IHealth {
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int damagePerTick = 10;
    [SerializeField] private int damageTicks = 3;

    private int currentHealth;
    private bool damaging = false;

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

        StartCoroutine(DamageOverTimeEffect());

        OnHPPctChanged(CurrentHpPct);

        if (CurrentHpPct <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }

    private IEnumerator DamageOverTimeEffect() {
        if (!damaging) {
            damaging = true;

            for (int i = 0 ; i < damageTicks; i++) {
                TakeDamage(damagePerTick);

                yield return new WaitForSeconds(0.5f);
            }

            damaging = false;
        }
    }
}