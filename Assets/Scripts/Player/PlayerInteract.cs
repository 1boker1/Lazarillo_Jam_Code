using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    [HideInInspector] public Interactable CurrentInteractable;

    public LayerMask    InteractLayer;
    public float        InteractDist;

    public void Interact ()
    {
        if (CurrentInteractable) CurrentInteractable.OnInteract();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray newRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(newRay, out hit, InteractDist, InteractLayer))
        {
            if (!CurrentInteractable)
            {
                CurrentInteractable = hit.collider.GetComponent<Interactable>();
                CurrentInteractable.OnEnter();
                PlayerManager._UI.ShowInteractText(CurrentInteractable.InteractMessage);

            }
            else if (CurrentInteractable != hit.collider.GetComponent<Interactable>())
            {
                CurrentInteractable = hit.collider.GetComponent<Interactable>();
                CurrentInteractable.OnEnter();
                PlayerManager._UI.ShowInteractText(CurrentInteractable.InteractMessage);
            }
        }
        else
        {
            if (CurrentInteractable)
            {
                CurrentInteractable.OnExit();
                PlayerManager._UI.HideInteractText();
                CurrentInteractable = null;
            }
        }
    }
}
