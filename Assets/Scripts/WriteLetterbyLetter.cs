using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteLetterbyLetter : MonoBehaviour {

    public float letterPause = 0.2f;

    public string message;
    public Text textComp;

    // Use this for initialization
    public void WriteLetter()
    {
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
