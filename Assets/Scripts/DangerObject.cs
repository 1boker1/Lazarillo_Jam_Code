using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    public float alertPerSecond = 10f;

    private void OnMouseDrag()
    {
        AlertIndicator.instance.AddAlertPoints(alertPerSecond * Time.deltaTime);
    }
}
