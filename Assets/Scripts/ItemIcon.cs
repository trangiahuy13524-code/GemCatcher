using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    private int currentAmount;

    public void Setup(ItemData item, int amount)
    {
        image.sprite = item.sprite;
        currentAmount = amount;
        UpdateText();
    }

    public void AddAmount(int amount)
    {
        currentAmount += amount;
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = currentAmount.ToString();
    }
}
