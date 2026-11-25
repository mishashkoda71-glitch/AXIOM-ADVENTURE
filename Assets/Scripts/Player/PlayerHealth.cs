
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float respawnDelay = 2f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDead { get; private set; }

    private CharacterController characterController;
    private PlayerController playerController;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        Debug.Log("AXIOM: Здоровье: " + CurrentHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        CurrentHealth = Mathf.Min(
            maxHealth,
            CurrentHealth + amount
        );

        Debug.Log("AXIOM: Игрок восстановил здоровье.");
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        if (characterController != null)
            characterController.enabled = false;

        if (playerController != null)
            playerController.enabled = false;

        Debug.Log("AXIOM: Игрок потерпел поражение.");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (Checkpoint.HasCheckpoint())
        {
            Checkpoint.RespawnPlayer(transform);
        }
        else
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
            yield break;
        }

        CurrentHealth = maxHealth;
        IsDead = false;

        if (characterController != null)
            characterController.enabled = true;

        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("AXIOM: Игрок снова в игре!");
    }
}
