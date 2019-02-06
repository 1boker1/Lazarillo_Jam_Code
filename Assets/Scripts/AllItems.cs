using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    public static AllItems instance;

    public List<Item> allItems = new List<Item>();
    public List<Item> badItems = new List<Item>();
    public List<GameObject> allBags = new List<GameObject>();
    public List<GameObject> allNPC = new List<GameObject>();
    public List<GameObject> allActiveNPC = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);
    }
}
