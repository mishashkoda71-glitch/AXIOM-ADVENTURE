
using System;
using UnityEngine;

public class GameSaveSystem : MonoBehaviour
{
    [Serializable]
    private class SaveData
    {
        public int unlockedLevel;
        public int crystals;
        public int mountainKey;
        public int secretCrystal;
        public int secretWorldUnlocked;
    }

    private const string SaveKey = "AXIOM_GAME_SAVE";

    public static void SaveGame()
    {
        SaveData data = new SaveData
        {
            unlockedLevel = PlayerPrefs.GetInt(
                "AXIOM_UNLOCKED_LEVEL", 1),

            crystals = PlayerPrefs.GetInt(
                "AXIOM_CRYSTALS", 0),

            mountainKey = PlayerPrefs.GetInt(
                "AXIOM_MOUNTAIN_KEY", 0),

            secretCrystal = PlayerPrefs.GetInt(
                "AXIOM_SECRET_CRYSTAL", 0),

            secretWorldUnlocked = PlayerPrefs.GetInt(
                "AXIOM_SECRET_WORLD_UNLOCKED", 0)
        };

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.Log("AXIOM: Прогресс сохранён!");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadBeforeFirstScene()
    {
        LoadGame();
    }

    public static void LoadGame()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            Debug.Log("AXIOM: Сохранение не найдено.");
            return;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null)
            return;

        PlayerPrefs.SetInt(
            "AXIOM_UNLOCKED_LEVEL", data.unlockedLevel);

        PlayerPrefs.SetInt(
            "AXIOM_CRYSTALS", data.crystals);

        PlayerPrefs.SetInt(
            "AXIOM_MOUNTAIN_KEY", data.mountainKey);

        PlayerPrefs.SetInt(
            "AXIOM_SECRET_CRYSTAL", data.secretCrystal);

        PlayerPrefs.SetInt(
            "AXIOM_SECRET_WORLD_UNLOCKED",
            data.secretWorldUnlocked);

        PlayerPrefs.Save();

        Debug.Log("AXIOM: Прогресс загружен!");
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();

        Debug.Log("AXIOM: Резервное сохранение удалено.");
    }
}
