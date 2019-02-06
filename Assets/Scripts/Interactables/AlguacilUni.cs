using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlguacilUni : Interactable {

    public GameObject canvas;
    public Text costText;

    public float Cost;

    public AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Exchange;

    public override void OnEnter()
    {
        canvas.SetActive(true);
        costText.text = Cost.ToString();

        audioSource.clip = Hmm;
        audioSource.Play();
    }

    public override void OnInteract()
    {
        if (PlayerManager._Instance.Money >= Cost)
        {

            audioSource.clip = Exchange;
            audioSource.Play();

            SceneManager.LoadScene(2);
        }
    }

    public override void OnExit()
    {
        canvas.SetActive(false);
    }

}
