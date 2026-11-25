using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public const int TotalLevels = 40;

    private const string UnlockedKey = "AXIOM_UNLOCKED_LEVEL";

    public int CurrentLevel { get; private set; }

    public int UnlockedLevel =>
        PlayerPrefs.GetInt(UnlockedKey, 1);

    private void Start()
    {
        CurrentLevel = GetLevelFromScene();
    }

    private int GetLevelFromScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level_") &&
            int.TryParse(sceneName.Substring(6), out int level))
        {
            return Mathf.Clamp(level, 1, TotalLevels);
        }

        return 1;
    }

    public void CompleteLevel()
    {
        int nextLevel = CurrentLevel + 1;

        if (nextLevel <= TotalLevels &&
            nextLevel > UnlockedLevel)
        {
            PlayerPrefs.SetInt(UnlockedKey, nextLevel);
            PlayerPrefs.Save();
        }

        if (CurrentLevel < TotalLevels)
        {
            LoadLevel(CurrentLevel + 1);
        }
        else
        {
            Debug.Log("AXIOM ADVENTURE COMPLETED!");
        }
    }

    public void LoadLevel(int level)
    {
        if (level < 1 || level > TotalLevels)
            return;

        if (level > UnlockedLevel)
            return;

        SceneManager.LoadScene("Level_" + level);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedKey);
        PlayerPrefs.Save();
    }
}
