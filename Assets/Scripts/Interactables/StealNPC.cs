using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealNPC : Interactable
{
    NPC npc;

    private void Start()
    {
        npc = GetComponent<NPC>();
    }

    public override void OnEnter()
    {
        if (!npc.canSteal)
        {
            InteractMessage = null;
        }
        else
        {
            InteractMessage = "Robar";

        }
    }

    public override void OnInteract()
    {
        if (npc.canSteal)
        {
            PlayerManager._Instance.ConsumeHunger(10);

            PlayerManager._Instance.OnStealing(npc);
            npc.stealing = true;

            StartCoroutine(PlayerCamera.instance.Transition(npc));
        }
    }

}
