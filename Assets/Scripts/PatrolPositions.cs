using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPositions : MonoBehaviour {

    public static PatrolPositions _Instance;
    public List<Transform> m_PatrolPositions;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
    void Start () {

        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != this.gameObject)
                m_PatrolPositions.Add (t);
        }
    }
	
}
