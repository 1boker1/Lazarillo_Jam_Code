using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadClickingCircle : MonoBehaviour
{
    public float alertPerSecond = 5;

    public GameObject normalCircle;
    public GameObject bigCircle;

    public Vector3 initialScale;

    public float timeToClose = 1f;
    public float timer = 0;

    public int alertPoints;

    void Start()
    {
        initialScale = bigCircle.transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, timeToClose);

        bigCircle.transform.localScale = Vector3.Lerp(initialScale, normalCircle.transform.localScale, timer / timeToClose);

        if (timer == timeToClose)
        {
            timer = 0;
            bigCircle.transform.localScale = initialScale;
        }
    }

    private void OnMouseDown()
    {
        float score = (timer / timeToClose) * 100;

        AlertIndicator.instance.AddAlertPoints(alertPoints * (1 - (timer / timeToClose)));

        transform.parent.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0)) return;

        AlertIndicator.instance.AddAlertPoints(alertPerSecond * Time.deltaTime);
    }
}
