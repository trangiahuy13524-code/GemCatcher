using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite sprite;

    public virtual void Execute(ItemData source)
    {

    }
}
