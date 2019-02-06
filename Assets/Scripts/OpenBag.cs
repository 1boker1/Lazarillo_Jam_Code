using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBag : MonoBehaviour
{
    public List<GameObject> objectsToGrab;

    public List<GameObject> otherObjects;
    public List<GameObject> badObjects;

    public bool isSliding;

    void Start()
    {
        SetRandomObjects();
    }

    public void SetRandomObjects()
    {
        foreach (GameObject random in otherObjects)
        {
            int idx = Random.Range(0, AllItems.instance.allItems.Count - 1);

            random.GetComponent<SpriteRenderer>().sprite = AllItems.instance.allItems[idx].image;
        }

        foreach (GameObject random in badObjects)
        {
            int idx = Random.Range(0, AllItems.instance.badItems.Count - 1);

            random.GetComponent<SpriteRenderer>().sprite = AllItems.instance.badItems[idx].image;
        }

        foreach (GameObject random in objectsToGrab)
        {
            int idx = Random.Range(0, AllItems.instance.allItems.Count - 1);

            random.GetComponent<AddItem>().itemToAdd = AllItems.instance.allItems[idx];
            random.GetComponent<AddItem>().spriteRenderer.sprite = AllItems.instance.allItems[idx].image;
            random.GetComponent<AddItem>().spriteRenderer.color = Color.green;

            random.GetComponentInChildren<Slider>().openBag = this;
        }
    }
}
