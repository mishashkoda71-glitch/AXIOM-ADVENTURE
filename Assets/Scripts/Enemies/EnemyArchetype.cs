
using UnityEngine;

public class EnemyArchetype : MonoBehaviour
{
    public enum EnemyType
    {
        Scout,
        Heavy,
        Hunter
    }

    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType = EnemyType.Scout;

    [Header("Scout Settings")]
    [SerializeField] private int scoutHealth = 60;
    [SerializeField] private float scoutSpeed = 5f;
    [SerializeField] private int scoutDamage = 10;

    [Header("Heavy Settings")]
    [SerializeField] private int heavyHealth = 200;
    [SerializeField] private float heavySpeed = 1.8f;
    [SerializeField] private int heavyDamage = 30;

    [Header("Hunter Settings")]
    [SerializeField] private int hunterHealth = 90;
    [SerializeField] private float hunterSpeed = 2.5f;
    [SerializeField] private int hunterDamage = 20;

    public int Health { get; private set; }
    public float Speed { get; private set; }
    public int Damage { get; private set; }

    private void Awake()
    {
        switch (enemyType)
        {
            case EnemyType.Scout:
                Health = scoutHealth;
                Speed = scoutSpeed;
                Damage = scoutDamage;
                break;

            case EnemyType.Heavy:
                Health = heavyHealth;
                Speed = heavySpeed;
                Damage = heavyDamage;
                break;

            case EnemyType.Hunter:
                Health = hunterHealth;
                Speed = hunterSpeed;
                Damage = hunterDamage;
                break;
        }
    }
}
