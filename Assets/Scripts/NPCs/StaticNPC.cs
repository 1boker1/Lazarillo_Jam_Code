using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticNPC : MonoBehaviour {

    public Transform quad;

    public GameObject InteractTell;
    public float DistToInteract;

    private void Update()
    {

        quad.LookAt(PlayerManager._Instance.transform.position);
        quad.eulerAngles = new Vector3(0,quad.transform.eulerAngles.y + 180,0);
        
        if (Vector3.Distance (transform.position,PlayerManager._Instance.transform.position) < DistToInteract && Vector3.Distance(transform.position, PlayerManager._Instance.transform.position) > 5)
        {
            if (!InteractTell.activeSelf)
                InteractTell.SetActive(true);
        }else
        {
            if (InteractTell.activeSelf)
                InteractTell.SetActive(false);
        }

    }

}
