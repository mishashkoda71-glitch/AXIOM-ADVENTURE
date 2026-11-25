using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelSelectButton : MonoBehaviour
{
    [Header("Level Settings")]
    [Range(1, 40)]
    [SerializeField] private int levelNumber = 1;

    private Button button;

    private const string UnlockedKey = "AXIOM_UNLOCKED_LEVEL";

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectLevel);
    }

    private void Start()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        int unlockedLevel =
            PlayerPrefs.GetInt(UnlockedKey, 1);

        button.interactable =
            levelNumber <= unlockedLevel;
    }

    private void SelectLevel()
    {
        int unlockedLevel =
            PlayerPrefs.GetInt(UnlockedKey, 1);

        if (levelNumber > unlockedLevel)
            return;

        SceneManager.LoadScene("Level_" + levelNumber);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(SelectLevel);
    }
}
