using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TextMeshProUGUI countDisplay;
    public ItemData itemData;
    
    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.sprite;
    }

    public void SetCount(int count)
    {
        countDisplay.text = count.ToString();
    }
}
