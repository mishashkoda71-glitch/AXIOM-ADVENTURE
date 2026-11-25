using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Death")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float destroyDelay = 0.2f;

    [Header("Rewards")]
    [SerializeField] private GameObject rewardPrefab;
    [Range(0f, 1f)]
    [SerializeField] private float rewardDropChance = 0.7f;
    [SerializeField] private float rewardHeight = 0.5f;

    public int CurrentHealth { get; private set; }

    private bool isDead;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        Debug.Log(
            "AXIOM: Здоровье противника: " + CurrentHealth
        );

        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("AXIOM: Противник побеждён!");

        if (deathEffect != null)
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );
        }

        DropReward();

        Destroy(gameObject, destroyDelay);
    }

    private void DropReward()
    {
        if (rewardPrefab == null)
            return;

        if (Random.value > rewardDropChance)
            return;

        Vector3 dropPosition =
            transform.position + Vector3.up * rewardHeight;

        Instantiate(
            rewardPrefab,
            dropPosition,
            Quaternion.identity
        );

        Debug.Log("AXIOM: Противник оставил награду!");
    }
}
