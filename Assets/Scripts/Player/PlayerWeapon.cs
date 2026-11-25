
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int damage = 35;
    [SerializeField] private float attackRange = 12f;
    [SerializeField] private float attackCooldown = 0.6f;

    [Header("Targeting")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask enemyLayers = ~0;

    private float nextAttackTime;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    // Вызывается кнопкой атаки на экране
    public void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        if (playerCamera == null)
            return;

        nextAttackTime = Time.time + attackCooldown;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            attackRange,
            enemyLayers
        ))
        {
            EnemyHealth enemy =
                hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                Debug.Log("AXIOM: Энергетический импульс попал!");
            }
        }
    }
}
