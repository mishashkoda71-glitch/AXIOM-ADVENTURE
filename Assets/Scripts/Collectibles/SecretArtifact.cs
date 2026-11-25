
using UnityEngine;

public class SecretArtifact : MonoBehaviour
{
    [Header("Artifact")]
    [SerializeField] private string artifactId = "Artifact_01";
    [SerializeField] private string artifactName = "Ancient Artifact";

    [Header("Visual Effects")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatHeight = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;
    private bool collected;

    private void Start()
    {
        startPosition = transform.position;

        if (IsCollected())
            gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime
        );

        Vector3 position = startPosition;
        position.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        collected = true;

        PlayerPrefs.SetInt(GetSaveKey(), 1);
        PlayerPrefs.SetInt("AXIOM_ARTIFACT_FIRST", 1);
        PlayerPrefs.SetInt("AXIOM_HAS_ANY_ARTIFACT", 1);
        PlayerPrefs.Save();
        GameSaveSystem.SaveGame();

        Debug.Log("Найден артефакт: " + artifactName);

        gameObject.SetActive(false);
    }

    private bool IsCollected()
    {
        return PlayerPrefs.GetInt(GetSaveKey(), 0) == 1;
    }

    private string GetSaveKey()
    {
        return "AXIOM_ARTIFACT_" + artifactId;
    }
}
