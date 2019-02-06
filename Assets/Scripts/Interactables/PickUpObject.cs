using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : Interactable
{

    public Item item;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.image;
    }

    private void Update()
    {
        transform.LookAt(Vector3.up + transform.position);
    }

    public override void OnEnter()
    {
        InteractMessage = ("Recojer " + item.name);

        if (item.foodValue != 0)
            InteractMessage += "\n comida = " + item.foodValue;
        if (item.moneyValue != 0)
            InteractMessage += "\n dinero = " + item.moneyValue;
    }

    public override void OnInteract()
    {
        PlayerManager._Instance.AddFood(item.foodValue);
        PlayerManager._Instance.AddMoney(item.moneyValue);
        PlayerManager._Instance.DropItems.Remove(this);
        gameObject.SetActive(false);
    }
}
