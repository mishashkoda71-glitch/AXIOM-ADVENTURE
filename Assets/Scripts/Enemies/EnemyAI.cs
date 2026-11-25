using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float normalAttackRange = 1.8f;
    [SerializeField] private float hunterAttackRange = 7f;

    [Header("Movement")]
    [SerializeField] private float defaultMoveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waypointDistance = 0.4f;
    [SerializeField] private float waitAtPoint = 1.5f;

    [Header("Attack")]
    [SerializeField] private int defaultDamage = 15;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private EnemyArchetype archetype;

    private int currentWaypoint;
    private float nextAttackTime;
    private float waitTimer;
    private bool waiting;

    private void Awake()
    {
        archetype = GetComponent<EnemyArchetype>();
    }

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
        {
            Patrol();
            return;
        }

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health == null || health.IsDead)
        {
            Patrol();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > detectionRange)
        {
            Patrol();
            return;
        }

        LookAtPlayer();

        float attackRange = normalAttackRange;

        if (archetype != null &&
            archetype.Type == EnemyArchetype.EnemyType.Hunter)
        {
            attackRange = hunterAttackRange;
        }

        if (distance > attackRange)
            ChasePlayer();
        else
            AttackPlayer(health);
    }

    private float GetMoveSpeed()
    {
        return archetype != null
            ? archetype.Speed
            : defaultMoveSpeed;
    }

    private void Patrol()
    {
        if (patrolPoints == null ||
            patrolPoints.Length == 0)
            return;

        Transform target = patrolPoints[currentWaypoint];

        if (target == null)
            return;

        Vector3 direction =
            target.position - transform.position;

        direction.y = 0f;

        if (direction.magnitude <= waypointDistance)
        {
            if (!waiting)
            {
                waiting = true;
                waitTimer = waitAtPoint;
            }

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                currentWaypoint =
                    (currentWaypoint + 1) %
                    patrolPoints.Length;

                waiting = false;
            }

            return;
        }

        waiting = false;

        RotateTowards(direction);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            GetMoveSpeed() * Time.deltaTime
        );
    }

    private void ChasePlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            ),
            GetMoveSpeed() * Time.deltaTime
        );
    }

    private void LookAtPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        RotateTowards(direction);
    }

    private void RotateTowards(Vector3 direction)
    {
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

    private void AttackPlayer(PlayerHealth health)
    {
        if (Time.time < nextAttackTime)
            return;

        int damage = archetype != null
            ? archetype.Damage
            : defaultDamage;

        health.TakeDamage(damage);

        nextAttackTime =
            Time.time + attackCooldown;

        Debug.Log("AXIOM: Противник атаковал игрока!");
    }
}
