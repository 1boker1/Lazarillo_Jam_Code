using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBag : MonoBehaviour
{
    public List<ClickToCircle> buttons = new List<ClickToCircle>();
    public List<Slider> sliders = new List<Slider>();


    public bool open;
    public bool isSliding;

    public GameObject openBag;

    void Update()
    {
        if (CheckDone() && !open)
        {
            open = true;

            StartCoroutine(OpenBag());
        }
    }

    private bool CheckDone()
    {
        foreach (ClickToCircle circle in buttons)
        {
            if (!circle.done) return false;
        }

        foreach (Slider slider in sliders)
        {
            if (!slider.finished) return false;
        }

        return true;
    }

    private IEnumerator OpenBag()
    {
        yield return new WaitForSeconds(0.1f);

        openBag.SetActive(true);


    }
}
