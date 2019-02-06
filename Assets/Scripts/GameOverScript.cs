using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

     [TextArea (1,20)]
    public List<string> Letter = new List<string>();

    public WriteLetterbyLetter write;

    public string option;

    string text;

    private void Start()
    {
        PlayerManager._Instance.OnStealing(null);

        write = GetComponent<WriteLetterbyLetter>();
        if (PlayerManager._Instance != null)
        {
            if (PlayerManager._Instance.Energy <= 0)
            {
                text = Letter[1];
            }
            else if (PlayerManager._Instance.HP <= 0)
            {
                text = Letter[2];
            }
            else
            {
                text = Letter[0];
            }
        }
        write.message = text;
        write.WriteLetter();
    }
}
