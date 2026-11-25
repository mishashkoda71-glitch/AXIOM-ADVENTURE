
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    [SerializeField] private int checkpointId = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        SaveCheckpoint();

        Debug.Log("AXIOM: Контрольная точка активирована!");
    }

    private void SaveCheckpoint()
    {
        string scene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("AXIOM_CHECKPOINT_SCENE", scene);
        PlayerPrefs.SetInt("AXIOM_CHECKPOINT_ID", checkpointId);

        Vector3 position = transform.position;

        PlayerPrefs.SetFloat("AXIOM_CHECKPOINT_X", position.x);
        PlayerPrefs.SetFloat("AXIOM_CHECKPOINT_Y", position.y + 1f);
        PlayerPrefs.SetFloat("AXIOM_CHECKPOINT_Z", position.z);

        PlayerPrefs.Save();

        GameSaveSystem.SaveGame();
    }

    public static void RespawnPlayer(Transform player)
    {
        if (player == null)
            return;

        string savedScene = PlayerPrefs.GetString(
            "AXIOM_CHECKPOINT_SCENE", "");

        if (string.IsNullOrEmpty(savedScene))
            return;

        string currentScene = SceneManager.GetActiveScene().name;

        if (savedScene != currentScene)
        {
            SceneManager.LoadScene(savedScene);
            return;
        }

        Vector3 position = new Vector3(
            PlayerPrefs.GetFloat("AXIOM_CHECKPOINT_X"),
            PlayerPrefs.GetFloat("AXIOM_CHECKPOINT_Y"),
            PlayerPrefs.GetFloat("AXIOM_CHECKPOINT_Z")
        );

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = position;

        if (controller != null)
            controller.enabled = true;
    }

    public static bool HasCheckpoint()
    {
        return PlayerPrefs.HasKey("AXIOM_CHECKPOINT_SCENE");
    }
}
