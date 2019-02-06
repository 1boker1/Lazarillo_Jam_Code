using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public Image sprite;

    public Text name;
    public Text description;
    public Text value;

    public Item item;

    public void SetUp(Item _item)
    {
        this.item = _item;

        sprite.sprite = item.image;
        sprite.color = Color.white;
        name.text = item.name;
        description.text = item.description;
        value.text = item.foodValue == 0 ? item.moneyValue.ToString() : item.foodValue.ToString();
    }
}
