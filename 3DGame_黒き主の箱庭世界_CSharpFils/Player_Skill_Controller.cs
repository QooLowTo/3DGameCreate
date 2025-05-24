using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Skill_Controller : MonoBehaviour
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

    private PlayerInput plain;//インプットシステム

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plain = gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SkillOn(InputAction.CallbackContext callback)
    { 
     if (!callback.performed) return;

        if (!skillOn)
        {
            plain.SwitchCurrentActionMap("SkillMode");
            skillPos.SetActive(true);
            skillOn = true;
        }
        else
        {
            plain.SwitchCurrentActionMap("Player");
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
            //removeElementNum--;
          
            removeCubeList.RemoveAt(0);

            removeCubeList.Add(removeCube.GetComponent<PlayerCubeController>());
            removeCubeList[0].DestroyCube();
        }
     
        //attackCubeList[0].transform.rotation = skillPos.transform.rotation;
    }
}
