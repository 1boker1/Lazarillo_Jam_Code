using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFood : Interactable {

    public float MoneyCost;
    public float FoodNum;

    public GameObject canvas;

    public Text MoneyCostTXT;
    public Text FoodNumTXT;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;


    public override void OnEnter()
    {
        canvas.SetActive(true);

        MoneyCostTXT.text = MoneyCost.ToString();
        FoodNumTXT.text = FoodNum.ToString();

        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        if (PlayerManager._Instance.Money >= MoneyCost)
        {
            PlayerManager._Instance.AddFood(FoodNum);
            PlayerManager._Instance.AddMoney(-MoneyCost);

            audioSource.clip = Exchange;
            audioSource.Play();
        }
    }

    public override void OnExit()
    {
        canvas.SetActive(false);
    }
}
