using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellFood : Interactable
{

    public float FoodCost;
    public float MoneyNum;

    public GameObject canvas;

    public Text MoneyNumTXT;
    public Text FoodCostTXT;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;

    public override void OnEnter()
    {
        canvas.SetActive(true);

        MoneyNumTXT.text = MoneyNum.ToString();
        FoodCostTXT.text = FoodCost.ToString();

        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        if (PlayerManager._Instance.Food >= FoodCost)
        {
            PlayerManager._Instance.AddFood(-FoodCost);
            PlayerManager._Instance.AddMoney(MoneyNum);

            audioSource.clip = Exchange;
            audioSource.Play();
        }
    }

    public override void OnExit()
    {
        canvas.SetActive(false);
    }

}
