using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertIndicator : MonoBehaviour
{
    public static AlertIndicator instance;

    public GameObject finishScreen;

    public List<Item> stealedObjects;

    public float maxAlert = 100f;

    public float actualAlert = 0;
    public float alertPerSecond = 2f;
    public float timer = 0f;

    public Color danger;
    public Color normal;
    public Color safe;

    public Image alertImage;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(Shake());
    }

    private void Update()
    {
        if (actualAlert >= maxAlert) ExitMinigame();

        actualAlert = Mathf.Clamp(actualAlert + (alertPerSecond * Time.deltaTime), 0, maxAlert);

        alertImage.fillAmount = actualAlert / maxAlert;

        ChangeColor(alertImage.fillAmount * 100);
    }

    public void ChangeColor(float percentage)
    {
        if (percentage > 90) alertImage.color = danger;
        else if (percentage > 50) alertImage.color = normal;
        else alertImage.color = safe;
    }

    public void AddAlertPoints(float amount)
    {
        actualAlert = Mathf.Clamp(actualAlert + amount, 0, maxAlert);
    }

    public void FinishMinigame()
    {
        stealedObjects.Clear();
        actualAlert = 0;
        alertPerSecond = 0;
        finishScreen.SetActive(false);
    }

    public void StartMinigame(float alertPerSecond)
    {
        alertImage.fillAmount = 0;
        actualAlert = 0;
        this.alertPerSecond = alertPerSecond;

        gameObject.SetActive(true);
    }

    public void ExitMinigame()
    {
        if (actualAlert == maxAlert)
        {
            ReturnToGame();
        }
        else
        {
            finishScreen.SetActive(true);


            if (stealedObjects.Count != 0)
            {
                foreach (Item item in stealedObjects)
                {
                    if (item.moneyValue != 0)
                    {
                        PlayerManager._Instance.AddMoney(item.moneyValue);
                    }
                    else if (item.foodValue != 0)
                    {
                        PlayerManager._Instance.AddFood(item.foodValue);
                    }
                    else
                    {
                        PlayerManager._Instance.PlayerItems.Add(item);
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }

    public void ReturnToGame()
    {
        PlayerCamera.instance.TriggerTransition((int)actualAlert);

        FinishMinigame();
    }

    public IEnumerator Shake()
    {
        while (true)
        {
            Vector3 startPosition = transform.localPosition;

            for (int i = 0; i < 10; i++)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * actualAlert / 10;

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = startPosition;
        }
    }
}
