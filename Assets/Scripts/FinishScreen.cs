using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishScreen : MonoBehaviour
{
    public List<GameObject> dropwdown;

    public GameObject nothingStolen;

    private void OnEnable()
    {
        InitScreen();
    }

    public void InitScreen()
    {
        for (int i = 0; i < AlertIndicator.instance.stealedObjects.Count; i++)
        {
            dropwdown[i].GetComponent<ItemDescription>().SetUp(AlertIndicator.instance.stealedObjects[i]);
            dropwdown[i].SetActive(true);
        }

        if (AlertIndicator.instance.stealedObjects.Count == 0)
        {
            nothingStolen.SetActive(true);
        }
    }

    public void ResetScreen()
    {
        nothingStolen.SetActive(false);

        foreach (GameObject item in dropwdown)
        {
            item.SetActive(false);
        }

        AlertIndicator.instance.stealedObjects.Clear();
    }
    private void OnDisable()
    {
        ResetScreen();
    }


}
