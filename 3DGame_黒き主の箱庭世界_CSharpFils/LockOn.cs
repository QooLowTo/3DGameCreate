
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
 
    private Player_Battle_Controller playerBattleCon;
   
    private Player_Camera_Controller playerCameraCon;
    [SerializeField]
    private GameObject findPlayerObj;

    private CinemachineVirtualCameraBase mainCineVir;
    [SerializeField]
    private GameObject findMainCineVir;

    private CinemachineVirtualCameraBase lockOnCineVir;
    [SerializeField]
    private GameObject findLookCineVir;

    [SerializeField]
    private List<Transform> targetTransform = new List<Transform>();

    bool firstLockOn;

    public List<Transform> TargetTransform { get => targetTransform; set => targetTransform = value; }
    public CinemachineVirtualCameraBase LockOnCineVir { get => lockOnCineVir; set => lockOnCineVir = value; }

    void Start()
    {
        playerBattleCon = findPlayerObj.GetComponent<Player_Battle_Controller>();

        playerCameraCon = findPlayerObj.GetComponent <Player_Camera_Controller>();

        mainCineVir = findMainCineVir.GetComponent<CinemachineVirtualCameraBase>();

        lockOnCineVir = findLookCineVir.GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
        if (!lockOnCineVir.enabled) return;


        if (targetTransform[0] != null)
        {
            lockOnCineVir.LookAt = targetTransform[0];


            var distanceToTarget = Vector3.Distance(findPlayerObj.transform.position, targetTransform[0].position);

            //マジックナンバー発見
            if (distanceToTarget < 2.5f)
            {
                playerBattleCon.Approached = true;
            }
            else
            {
                playerBattleCon.Approached = false;
            }

            if (playerBattleCon.AttackingLook || playerBattleCon.Avoidancing)
            {
                if (playerBattleCon.ForwardAvoidancing) return;

                var lookpos = new Vector3(targetTransform[0].position.x, playerBattleCon.transform.position.y, targetTransform[0].position.z);
                playerBattleCon.transform.DOLookAt(lookpos, 0.5f);
            }
        }
        else
        {
            playerCameraCon.OffLockOn();

            lockOnCineVir.enabled = false;
        }
      
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            //マジックナンバー発見
            playerBattleCon.SoundManager.OneShot_Player_Sound(10);//ロックオンサウンド

            targetTransform.Add(collider.transform);

            playerCameraCon.GetTarget = true;

            lockOnCineVir.enabled = true;

            if (!firstLockOn)
            { 
             firstLockOn = true;
            }
           
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
