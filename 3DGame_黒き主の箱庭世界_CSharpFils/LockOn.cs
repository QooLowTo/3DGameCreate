
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// 敵をロックオンできるようにするクラスです。
/// </summary>
public class LockOn : MonoBehaviour
{
 
    private Player_Battle_Controller placon;
   
    private Player_Camera_Controller placam;
    [SerializeField]
    private GameObject findPla;

    private CinemachineVirtualCameraBase mainCineVir;
    [SerializeField]
    private GameObject findMainCineVir;

    private CinemachineVirtualCameraBase lockOnCineVir;
    [SerializeField]
    private GameObject findLookCineVir;

    [SerializeField]
    private List<Transform> targetTramsform = new List<Transform>();

    bool firstLockOn;

    public List<Transform> TargetTramsform { get => targetTramsform; set => targetTramsform = value; }
    public CinemachineVirtualCameraBase LockOnCineVir { get => lockOnCineVir; set => lockOnCineVir = value; }

    // Start is called before the first frame update
    void Start()
    {
        placon = findPla.GetComponent<Player_Battle_Controller>();

        placam = findPla.GetComponent <Player_Camera_Controller>();

        mainCineVir = findMainCineVir.GetComponent<CinemachineVirtualCameraBase>();

        lockOnCineVir = findLookCineVir.GetComponent<CinemachineVirtualCameraBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockOnCineVir.enabled) return;


        if (targetTramsform[0] != null)
        {
            lockOnCineVir.LookAt = targetTramsform[0];


            var distanceToTarget = Vector3.Distance(findPla.transform.position, targetTramsform[0].position);

            if (distanceToTarget < 2.5f)
            {
                placon.Approached = true;
            }
            else
            {
                placon.Approached = false;
            }

            if (placon.AttackingLook || placon.Avoidancing)
            {
                if (placon.ForwardAvoidancing) return;

                var lookpos = new Vector3(targetTramsform[0].position.x, placon.transform.position.y, targetTramsform[0].position.z);
                placon.transform.DOLookAt(lookpos, 0.5f);
            }
        }
        else
        {
            placam.OffLockOn();

            lockOnCineVir.enabled = false;
        }



        //if (lockOnCineVir.LookAt &&firstLockOn)
        //{
        //    targetTramsform.Remove(targetTramsform[0]);
        //    lockOnCineVir.enabled = false;
        //    if (placam.GetTarget)
        //    { 
        //    placam.GetTarget = false;
        //    }
        //}
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            placon.SoundManager.OneShot_Player_Sound(10);//ロックオンサウンド

            targetTramsform.Add(collider.transform);

            placam.GetTarget = true;

            lockOnCineVir.enabled = true;

            if (!firstLockOn)
            { 
             firstLockOn = true;
            }
           
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
