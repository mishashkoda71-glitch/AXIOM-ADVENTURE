using UnityEngine;
using UnityEngine.UI;

public class CrystalCounterUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Text crystalText;
    [SerializeField] private string prefix = "💎 ";

    private int lastAmount = -1;

    private void Start()
    {
        UpdateCounter();
    }

    private void Update()
    {
        if (lastAmount != CrystalInventory.Amount)
        {
            UpdateCounter();
        }
    }

    private void UpdateCounter()
    {
        lastAmount = CrystalInventory.Amount;

        if (crystalText != null)
        {
            crystalText.text = prefix + lastAmount;
        }
    }
}
