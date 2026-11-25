
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactJournalUI : MonoBehaviour
{
    [Serializable]
    public class ArtifactEntry
    {
        public string id;
        public string displayName;
        [TextArea] public string description;
    }

    [Header("Journal")]
    [SerializeField] private Text journalText;
    [SerializeField] private List<ArtifactEntry> artifacts =
        new List<ArtifactEntry>();

    private void OnEnable()
    {
        RefreshJournal();
    }

    public void RefreshJournal()
    {
        if (journalText == null)
            return;

        int found = 0;
        string result = "AXIOM — ЖУРНАЛ ТАЙН\n\n";

        foreach (ArtifactEntry artifact in artifacts)
        {
            if (string.IsNullOrEmpty(artifact.id))
                continue;

            bool collected = PlayerPrefs.GetInt(
                "AXIOM_ARTIFACT_" + artifact.id, 0) == 1;

            if (collected)
            {
                found++;
                result += "[НАЙДЕНО] " + artifact.displayName +
                          "\n" + artifact.description + "\n\n";
            }
            else
            {
                result += "[???] Неизвестная находка\n\n";
            }
        }

        result += "\nНайдено: " + found +
                  " / " + artifacts.Count;

        journalText.text = result;
    }
}
