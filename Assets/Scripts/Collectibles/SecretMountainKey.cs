
using UnityEngine;

public class SecretMountainKey : MonoBehaviour
{
    private const string KeyFound = "AXIOM_MOUNTAIN_KEY";

    [Header("Key Effects")]
    [SerializeField] private float rotationSpeed = 75f;
    [SerializeField] private float floatHeight = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;
    private bool collected;

    public static bool HasKey =>
        PlayerPrefs.GetInt(KeyFound, 0) == 1;

    private void Start()
    {
        startPosition = transform.position;

        if (HasKey)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        Vector3 position = startPosition;
        position.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        collected = true;

        PlayerPrefs.SetInt(KeyFound, 1);
        PlayerPrefs.Save();
        GameSaveSystem.SaveGame();

        Debug.Log("Вы открыли секретную гору!");

        Destroy(gameObject);
    }
}
