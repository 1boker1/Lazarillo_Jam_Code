using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera playerCamera;
    public float fov;

    public float orthographicSize = .25f;

    public Vector3 cameraPosition;

    public Animation fade;

    public AnimationClip fadeIn;
    public AnimationClip fadeOut;

    public NPC stealingNPC;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        playerCamera = Camera.main;
        fov = playerCamera.fieldOfView;
    }

    void Start()
    {
        cameraPosition = playerCamera.transform.localPosition;
    }

    public IEnumerator Transition(NPC npc)
    {
        stealingNPC = npc;
        Bag bag = npc.bag.GetComponent<Bag>(); ;

        fade.Play(fadeIn.name);

        yield return new WaitForSeconds(fadeIn.length);

        bag.camera.enabled = true;
        bag.spriteRenderer.enabled = false;

        fade.Play(fadeOut.name);

        bag.closedBag.gameObject.SetActive(true);

        yield return new WaitForSeconds(fadeOut.length * 0.5f);

        AlertIndicator.instance.StartMinigame(npc.AlertperSecond);
    }

    public IEnumerator ExitTransition(int score)
    {
        fade.Play(fadeOut.name);

        Bag bag = stealingNPC.bag.GetComponent<Bag>();

        bag.camera.enabled = false;

        bag.spriteRenderer.enabled = true;

        bag.closedBag.GetComponent<ClosedBag>().openBag.gameObject.SetActive(false);
        bag.closedBag.gameObject.SetActive(false);

        stealingNPC = null;

        playerCamera.orthographic = false;
        playerCamera.fieldOfView = fov;

        transform.parent = PlayerManager._Instance.transform;
        transform.localPosition = cameraPosition;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return new WaitForSeconds(fadeOut.length);

        PlayerManager._Instance.OnWalking(score);
    }

    public void TriggerTransition(int score)
    {
        StartCoroutine(ExitTransition(score));
    }
}
