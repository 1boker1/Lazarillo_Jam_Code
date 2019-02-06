using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPositions : MonoBehaviour {

    public static SpawnerPositions _Instance;
    int children;
    public List<Transform> m_SpawnerPositions;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;

        }
        children = transform.childCount;
        for (int i = 0; i < children; i++)
            m_SpawnerPositions.Add(transform.GetChild(i));
    }
}
