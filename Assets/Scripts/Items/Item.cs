using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite image;

    public int foodValue;
    public int moneyValue;

    [TextArea (2,5)]
    public string description;
}
