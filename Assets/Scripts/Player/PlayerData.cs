using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : Base
{
    public override bool isPlayer => true;
    [SerializeField] TextMeshProUGUI pointDisplayText;
    [SerializeField] AudioSource audioSource;
    public int point { get; private set; } = 0;
    public bool playAudio;
    public static PlayerData instance;

    //Dictionary<ItemData, int> inventory = new();
    //Dictionary<ItemData, ItemUI> inventoryUI = new();
    //[SerializeField] ItemUI itemUIprefab;
    //[SerializeField] Transform gridLayoutParent;
    private void Start()
    {
        instance = this;
    }

    public void AddPoint(int num)
    {
        if (num < 0) num = 0;
        point += num;
        pointDisplayText.text = point.ToString();
    }

    public void PlayAudio()
    {
        if (playAudio)
            audioSource.Play();
    }

    public void ResetPoint()
    {
        point = 0;
        pointDisplayText.text = point.ToString();
    }

    //public void AddInventory(ItemData itemData)
    //{
    //    if (inventory.ContainsKey(itemData))
    //    {
    //        inventory[itemData] += 1;
    //    }
    //    else
    //    {
    //        inventory.Add(itemData, 1);
    //    }
    //    if (inventoryUI.ContainsKey(itemData))
    //    {
    //        ItemUI ui = inventoryUI[itemData];
    //        if (ui != null)
    //        {
    //            ui.SetCount(inventory[itemData]);
    //        }
    //        else
    //        {
    //            Debug.LogError("No UI gameobject!");
    //        }
    //    }
    //    else
    //    {
    //        ItemUI itemUI = Instantiate(itemUIprefab);
    //        itemUI.SetItemData(itemData);
    //        itemUI.SetCount(1);
    //        inventoryUI.Add(itemData, itemUI);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
