using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    [Range(1, 1000)] public int value = 1;
}
