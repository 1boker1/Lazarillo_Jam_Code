using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToCircle : MonoBehaviour
{
    public GameObject normalCircle;
    public GameObject bigCircle;

    public GameObject followingSlider;

    public GameObject particles;

    public Vector3 initialScale;

    public float timeToClose = 2f;
    public float timer = 0;

    public int alertPoints;

    public int perfect = 90;
    public int good = 60;
    public int careful = 30;
    public int fail = 0;

    public bool clicked = false;
    public bool done;

    void Start()
    {
        initialScale = bigCircle.transform.localScale;
    }

    void Update()
    {
        if (clicked) return;

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
        if (clicked) return;

        clicked = true;

        float score = (timer / timeToClose) * 100;

        if (score >= perfect)
        {

        }
        else if (score >= good)
        {
            AlertIndicator.instance.AddAlertPoints(alertPoints * (1 - (timer / timeToClose)));
        }
        else if (score >= careful)
        {
            AlertIndicator.instance.AddAlertPoints(alertPoints * (1 - (timer / timeToClose)));
        }
        else
        {
            AlertIndicator.instance.AddAlertPoints(alertPoints);
        }

        done = true;

        if (followingSlider != null) followingSlider.SetActive(true);
        if (particles != null) Instantiate(particles, transform.localPosition, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
