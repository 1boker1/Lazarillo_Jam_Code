using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquireSleep : Interactable {

    public float Cost;
    public GameObject npcCanvas;
    public Text cost;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;

    public override void OnEnter()
    {
        npcCanvas.SetActive (true);
        cost.text = Cost.ToString ();

        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        if (PlayerManager._Instance.Food >= Cost)
        {
            PlayerManager._Instance.OnSleeping(Cost, PlayerManager._Instance.SquireEnergy, "Este tio esta peor que yo. No dejava de mmirar la comida que llevaba encima. \n Al menos hoy duermo en una cama");
            audioSource.clip = Exchange;
            audioSource.Play();
        }
    }

    public override void OnExit()
    {
        npcCanvas.SetActive(false);
    }
}
