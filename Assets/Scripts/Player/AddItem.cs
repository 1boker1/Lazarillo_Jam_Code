using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Item itemToAdd;
    public Slider slider;

    private void OnDisable()
    {
        if (!slider.finished) return;

        if (AlertIndicator.instance != null)
            AlertIndicator.instance.stealedObjects.Add(itemToAdd);
    }
}
