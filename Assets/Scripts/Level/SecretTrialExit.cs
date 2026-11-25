
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretTrialExit : MonoBehaviour
{
    [Header("Secret World")]
    [SerializeField] private string secretWorldScene = "Secret_World";

    private bool completed;

    private const string WorldUnlockedKey =
        "AXIOM_SECRET_WORLD_UNLOCKED";

    public static bool IsWorldUnlocked =>
        PlayerPrefs.GetInt(WorldUnlockedKey, 0) == 1;

    private void OnTriggerEnter(Collider other)
    {
        if (completed || !other.CompareTag("Player"))
            return;

        completed = true;

        PlayerPrefs.SetInt(WorldUnlockedKey, 1);
        PlayerPrefs.Save();

        Debug.Log("Секретное испытание пройдено!");
        Debug.Log("Открыт AXIOM SECRET WORLD!");

        SceneManager.LoadScene(secretWorldScene);
    }
}
