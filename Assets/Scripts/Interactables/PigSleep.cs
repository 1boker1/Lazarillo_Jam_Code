using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSleep : Interactable {

    public float cost;

    public GameObject npcCanvas;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;

    public override void OnEnter()
    {
        npcCanvas.SetActive(true);

        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        PlayerManager._Instance.OnSleeping(cost, PlayerManager._Instance.PigEnergy, "Hoy duermo con los cerdos. Espero que no se me coman la comida. \n Al menos no me robaran");

        audioSource.clip = Exchange;
        audioSource.Play();
    }

    public override void OnExit()
    {
        npcCanvas.SetActive(false);
    }
}
