using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class FallingObject : Base
{
    public override bool isPlayer => false;
    [SerializeField] ItemData itemData;
    [SerializeField] SpriteRenderer spriteRenderer;
    //public float lifeCycleTime = 3;
    //private float curTime;

    public void SetItemData(ItemData item)
    {
        itemData = item;
        spriteRenderer.sprite = itemData.sprite;
    }

    Stopwatch sw = new Stopwatch();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Base @base = collision.gameObject.GetComponent<Base>();
        if (@base != null)
        {
            if (@base.isPlayer)
            {
                PlayerData.instance.PlayAudio();

                PlayerData.instance.AddPoint(itemData.value * WorldManager.Instance.CurrentCombo);
                WorldManager.Instance.CurrentCombo++;
            }
            else
            {
                return;
            }
        }
        else
        {
            WorldManager.Instance.CurrentCombo = 1;
        }
        WorldManager.spawnedObjects.Remove(this);
        WorldManager.Instance.ReturnToPool(this);
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