
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretMountainCrystal : MonoBehaviour
{
    [Header("Crystal Settings")]
    [SerializeField] private float rotationSpeed = 65f;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Secret Level")]
    [SerializeField] private string secretLevelName = "Secret_Trial";

    private Vector3 startPosition;
    private bool activated;

    private const string CrystalFoundKey = "AXIOM_SECRET_CRYSTAL";

    public static bool IsFound =>
        PlayerPrefs.GetInt(CrystalFoundKey, 0) == 1;

    private void Start()
    {
        startPosition = transform.position;

        if (IsFound)
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
        if (activated || !other.CompareTag("Player"))
            return;

        if (!SecretMountainKey.HasKey)
        {
            Debug.Log("Для этого кристалла нужен секретный ключ!");
            return;
        }

        activated = true;

        PlayerPrefs.SetInt(CrystalFoundKey, 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(secretLevelName);
    }
}
