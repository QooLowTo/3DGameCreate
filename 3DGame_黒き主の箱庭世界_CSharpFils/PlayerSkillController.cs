using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> attackCubeList = new List<GameObject>();

    [SerializeField]
    private List<PlayerCubeController> removeCubeList = new List<PlayerCubeController>();

    [SerializeField]
    private GameObject skillPos;

    [SerializeField]
    int removeElementNum = 0;

    private bool skillOn = false;

    private PlayerInput playerInput;//インプットシステム

    void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
    }


    public void SkillOn(InputAction.CallbackContext callback)
    { 
     if (!callback.performed) return;

        if (!skillOn)
        {
            playerInput.SwitchCurrentActionMap("SkillMode");
            skillPos.SetActive(true);
            skillOn = true;
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            skillPos.SetActive(false);
            skillOn = false;
        }

    }

    public void Skill(InputAction.CallbackContext callback)
    {
        if (!callback.performed || !skillOn) return;

        GameObject removeCube;

       removeCube = Instantiate(attackCubeList[0], skillPos.transform.position,Quaternion.identity);


        if (removeElementNum < 3)
        {
            removeCubeList.Add(removeCube.GetComponent<PlayerCubeController>());
            removeElementNum++;
        }
        else
        {
          
            removeCubeList.RemoveAt(0);

            removeCubeList.Add(removeCube.GetComponent<PlayerCubeController>());
            removeCubeList[0].DestroyCube();
        }
     
    }
}
