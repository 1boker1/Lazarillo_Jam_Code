using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public Image HPImage;
    public Image HungerImage;
    public Image StaminaImage;
    public Image EatImage;
    public Image SleepImage;

    public Text FoodText;
    public Text MoneyText;

    public Text InteractText;

    public Text SleepText;



    public GameObject StealUI;
    public GameObject WalkingUI;

    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        RefreshUI();
    }

    public void RefreshEatImage(float cuantity)
    {
        if (!EatImage.enabled)
            EatImage.enabled = true;
        EatImage.fillAmount = cuantity;
    }
    public void HideEatImage()
    {
        EatImage.enabled = false;
    }

    public void EnableWalkingUI()
    {
        StealUI.SetActive(false);
        WalkingUI.SetActive(true);
    }

    public void DesableWalkingUI()
    {
        StealUI.SetActive(true);
        WalkingUI.SetActive(false);
    }

    public void ShowInteractText(string txt)
    {
        InteractText.enabled = true;
        InteractText.text = txt;
    }

    public void HideInteractText()
    {
        InteractText.enabled = false;
    }

    public void RefreshUI()
    {
        HPImage.fillAmount = PlayerManager._Instance.HP / PlayerManager._Instance.MaxHP;
        HungerImage.fillAmount = PlayerManager._Instance.Energy / PlayerManager._Instance.MaxEnergy;
        MoneyText.text = "Dinero = " + PlayerManager._Instance.Money.ToString();
        FoodText.text = "Comida = " + PlayerManager._Instance.Food.ToString();
    }

    public void RefreshSleep()
    {
        if (SleepImage != null)
            SleepImage.fillAmount = PlayerManager._Instance.sleep / PlayerManager._Instance.TimeToSleep;
    }

    public void HideStamina()
    {
        StaminaImage.enabled = false;
    }

    public void RefreshStamina()
    {
        if (!StaminaImage.enabled)
        {
            StaminaImage.enabled = true;
        }

        StaminaImage.fillAmount = PlayerManager._Instance.Stamina / PlayerManager._Instance.MaxStamina;
    }

    public void MakeFadeIn(string txt)
    {
        PlayerManager._Instance.day++;
        SleepText.text = txt;
        SleepText.text += "\n Dia : " + PlayerManager._Instance.day.ToString();

        anim.Play();
        StartCoroutine(NightCicleCO());
    }

    bool onNight;
    IEnumerator NightCicleCO()
    {
        yield return new WaitForSeconds(anim.clip.length / 2);
        RefreshUI();
        yield return new WaitForSeconds(anim.clip.length / 2);
        PlayerManager._Instance.WakeUp();
    }
}
