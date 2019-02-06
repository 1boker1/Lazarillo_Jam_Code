using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCheck : MonoBehaviour
{

    private void Update()
    {
        if (PlayerManager._Instance.Energy <= 0 || PlayerManager._Instance.HP <= 0)
        {
            PlayerManager._Instance.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
