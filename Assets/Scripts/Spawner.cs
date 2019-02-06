using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int RandomNPC;
    public float counterSpawn;
    public float timeSpawn;
    GameObject newNPC;
    public int maxNPC;


    void Start ()
    {

        counterSpawn = 0;
        RandomNPC = Random.Range(0, AllItems.instance.allNPC.Count - 1);
        newNPC = Instantiate(AllItems.instance.allNPC[RandomNPC], transform.position, transform.rotation);
        AllItems.instance.allActiveNPC.Add(newNPC);



    }
	
	
	void Update ()
    {
        counterSpawn += Time.deltaTime;
        if(counterSpawn >= timeSpawn && AllItems.instance.allActiveNPC.Count < maxNPC)
        {
            RandomNPC = Random.Range(0, AllItems.instance.allNPC.Count - 1);
            newNPC = Instantiate(AllItems.instance.allNPC[RandomNPC], transform.position, transform.rotation);
            AllItems.instance.allActiveNPC.Add(newNPC);
            counterSpawn = 0;
        }
	}
}
