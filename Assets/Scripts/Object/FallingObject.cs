using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] SpriteRenderer spriteRenderer;
    //public float lifeCycleTime = 3;
    //private float curTime;

    public void SetItemData(ItemData item)
    {
        itemData = item;
        spriteRenderer.sprite = itemData.sprite;
    }

    void DoEffect(PlayerData player)
    {
        player.AddPoint(1);
        player.PlayAudio();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerData playerData = collision.gameObject.GetComponent<PlayerData>();
        if (playerData != null)
        {
            //playerData.AddInventory(itemData);
            DoEffect(playerData);
            
        }
        Destroy(gameObject);
    }

    //private void Update()
    //{
    //    if(curTime >= lifeCycleTime)
    //    {
    //        Destroy(gameObject);
    //    }
    //    curTime += Time.deltaTime;
    //}
}
