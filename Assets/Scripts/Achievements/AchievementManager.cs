
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public enum AchievementType
    {
        CollectFirstCrystal,
        Collect10Crystals,
        ReachLevel10,
        ReachLevel20,
        ReachLevel40,
        FindSecretWorld,
        FindFirstArtifact
    }

    [Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        [TextArea] public string description;
        public AchievementType type;
    }

    [Header("Achievements")]
    [SerializeField] private List<Achievement> achievements =
        new List<Achievement>();

    [Header("UI")]
    [SerializeField] private Text achievementsText;

    private const string UnlockedPrefix = "AXIOM_ACHIEVEMENT_";

    private void OnEnable()
    {
        RefreshAchievements();
    }

    public void RefreshAchievements()
    {
        if (achievementsText == null)
            return;

        string result = "AXIOM — ДОСТИЖЕНИЯ\n\n";
        int unlocked = 0;

        foreach (Achievement achievement in achievements)
        {
            if (string.IsNullOrEmpty(achievement.id))
                continue;

            bool completed = IsCompleted(achievement.type);

            if (completed)
            {
                PlayerPrefs.SetInt(
                    UnlockedPrefix + achievement.id, 1);

                unlocked++;
                result += "[ПОЛУЧЕНО] " + achievement.title +
                          "\n" + achievement.description + "\n\n";
            }
            else
            {
                result += "[ЗАКРЫТО] " + achievement.title +
                          "\n" + achievement.description + "\n\n";
            }
        }

        result += "Открыто достижений: " + unlocked +
                  " / " + achievements.Count;

        achievementsText.text = result;
        PlayerPrefs.Save();
    }

    private bool IsCompleted(AchievementType type)
    {
        int crystals = PlayerPrefs.GetInt("AXIOM_CRYSTALS", 0);
        int level = PlayerPrefs.GetInt("AXIOM_UNLOCKED_LEVEL", 1);

        switch (type)
        {
            case AchievementType.CollectFirstCrystal:
                return crystals >= 1;

            case AchievementType.Collect10Crystals:
                return crystals >= 10;

            case AchievementType.ReachLevel10:
                return level >= 10;

            case AchievementType.ReachLevel20:
                return level >= 20;

            case AchievementType.ReachLevel40:
                return level >= 40;

            case AchievementType.FindSecretWorld:
                return PlayerPrefs.GetInt(
                    "AXIOM_SECRET_WORLD_UNLOCKED", 0) == 1;

            case AchievementType.FindFirstArtifact:
                return HasAnyArtifact();

            default:
                return false;
        }
    }

    private bool HasAnyArtifact()
    {
        return PlayerPrefs.GetInt(
            "AXIOM_ARTIFACT_FIRST", 0) == 1;
    }
}
