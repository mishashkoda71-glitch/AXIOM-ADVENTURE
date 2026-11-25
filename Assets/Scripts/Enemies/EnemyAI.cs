
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.8f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Attack")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private float nextAttackTime;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health == null || health.IsDead)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > detectionRange)
            return;

        LookAtPlayer();

        if (distance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer(health);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ChasePlayer()
    {
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void AttackPlayer(PlayerHealth health)
    {
        if (Time.time < nextAttackTime)
            return;

        health.TakeDamage(damage);
        nextAttackTime = Time.time + attackCooldown;

        Debug.Log("AXIOM: Противник атаковал игрока!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
