
using UnityEngine;

public class HealthCrystal : MonoBehaviour
{
    [Header("Healing")]
    [SerializeField] private int healingAmount = 30;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 70f;
    [SerializeField] private float floatHeight = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime
        );

        float offset = Mathf.Sin(Time.time * floatSpeed)
                       * floatHeight;

        transform.position = startPosition +
                             Vector3.up * offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        if (health.CurrentHealth >= health.MaxHealth)
            return;

        health.Heal(healingAmount);

        Debug.Log("AXIOM: Найден лечебный кристалл!");

        Destroy(gameObject);
    }
}
