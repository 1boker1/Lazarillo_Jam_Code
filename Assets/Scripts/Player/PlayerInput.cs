using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [HideInInspector]
    public Vector3 _MovementForward;
    public GameObject canvasPause;

    Vector3 GetForwardMovement()
    {
        float HMovement = Input.GetAxis("Horizontal");
        float VMovement = Input.GetAxis("Vertical");

        Vector3 moveDirSide = PlayerManager._Movement.transform.right * HMovement;
        Vector3 moveDirForward = PlayerManager._Movement.transform.forward * VMovement;

        Vector3 dir = new Vector3(moveDirSide.x + moveDirForward.x, 0, moveDirSide.z + moveDirForward.z);

        

        if (dir.magnitude > 1)
            return dir.normalized;
        else
            return dir;

        
    }


    void Update()
    {
        if (!PlayerManager._Instance.Stealing)
            NormalInput();
        
    }

    void NormalInput ()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (canvasPause.activeInHierarchy)
            {
                canvasPause.SetActive(false);
            }
            else
            {
                canvasPause.SetActive(true);
            }
        }
        #region MOVEMENT

        //MoveCamera
        PlayerManager._Movement.GetMouseInput(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        //2D MOVEMENT
        PlayerManager._Movement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            PlayerManager._Movement.StartJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            PlayerManager._Movement.EndJump();
        }

        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            if (PlayerManager._Instance.canRun)
                PlayerManager._Movement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            PlayerManager._Movement.Walk();
        }
        //INTERACT
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerManager._Interact.Interact();
        }
        //EAT
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerManager._Instance.StartEating();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            PlayerManager._Instance.StopEating();
        }

        #endregion
    }

}
