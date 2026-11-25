
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float damageCooldown = 1f;

    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        health.TakeDamage(damage);
        nextDamageTime = Time.time + damageCooldown;

        Debug.Log("AXIOM: Игрок получил урон от ловушки!");
    }
}
