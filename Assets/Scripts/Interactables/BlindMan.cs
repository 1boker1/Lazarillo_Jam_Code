using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindMan : Interactable {

    Item item;

    public Image ItemImage;
    public Text ItemValue;
    public Text Message;

    public GameObject Ok;
    public GameObject canvas;

    float itemValue;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;

    bool canBuy;

    void RandomizeItemValue ()
    {
        item = PlayerManager._Instance.PlayerItems[Random.Range(0, PlayerManager._Instance.PlayerItems.Capacity - 1)];
        itemValue = Random.Range(1, 10);
    }


    public override void OnEnter()
    {
        if (PlayerManager._Instance.PlayerItems.Count != 0)
        {
            Message.enabled = false;
            Ok.SetActive(true);

            RandomizeItemValue();
            ItemImage.sprite = item.image;
            ItemValue.text = itemValue.ToString ();

            
            canBuy = true;
        }
        else
        {
            Message.enabled = true;
            Ok.SetActive(false);

            Message.text = "Trae algo que valga la pena inutil!";
        }
        canvas.SetActive(true);
        
        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        if (canBuy)
        {
            PlayerManager._Instance.PlayerItems.Remove(item);
            PlayerManager._Instance.AddMoney(itemValue);

            audioSource.clip = Exchange;
            audioSource.Play();

            canBuy = false;
        }

    }


    public override void OnExit()
    {
        canvas.SetActive(false);

            canBuy = false;
    }
}
