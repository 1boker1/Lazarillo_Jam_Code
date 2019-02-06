using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInicio : MonoBehaviour {

    public int RandomNPC;
    GameObject newNPC;
    public int maxNPC;


    void Start()
    {
        RandomNPC = Random.Range(0, AllItems.instance.allNPC.Count - 1);
        newNPC = Instantiate(AllItems.instance.allNPC[RandomNPC], transform.position, transform.rotation);
        AllItems.instance.allActiveNPC.Add(newNPC);
    }
}
