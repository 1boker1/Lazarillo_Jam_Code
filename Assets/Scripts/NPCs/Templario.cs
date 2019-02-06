using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Templario : MonoBehaviour {

    public Transform quad;

	void Update ()
    {
        quad.LookAt(PlayerManager._Instance.transform.position);
        quad.eulerAngles = new Vector3(0, quad.transform.eulerAngles.y + 180, 0);
    }
}
