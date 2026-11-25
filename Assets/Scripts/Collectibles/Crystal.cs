
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [Header("Crystal Settings")]
    [SerializeField] private int value = 1;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;
    private bool collected;

    private void Start()
    {
        startPosition = transform.position;
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
        CrystalInventory.Add(value);

        Destroy(gameObject);
    }
}

public static class CrystalInventory
{
    private const string CrystalKey = "AXIOM_CRYSTALS";

    public static int Amount =>
        PlayerPrefs.GetInt(CrystalKey, 0);

    public static void Add(int amount)
    {
        if (amount <= 0)
            return;

        PlayerPrefs.SetInt(CrystalKey, Amount + amount);
        PlayerPrefs.Save();

        Debug.Log("Crystals collected: " + Amount);
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteKey(CrystalKey);
        PlayerPrefs.Save();
    }
}
