
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが敵にロックオンできるようにするクラスです。
/// </summary>
public class PlayerCameraController : MonoBehaviour
{

    private LockOn lookOn;

    [SerializeField]
    private Collider lookOnCollider;

    [SerializeField]
    private GameObject findLookOn;

    private float distanceToTarget;

    private GameObject target;

    bool getTarget = false;

    bool lockTaget = false;

    bool lockOut = false;

    [SerializeField]
    bool locking = false;

    public GameObject Target { get => target; set => target = value; }
    public bool GetTarget { get => getTarget; set => getTarget = value; }
    public bool Locking { get => locking; set => locking = value; }

    void Start()
    {
        lookOn = findLookOn.GetComponent<LockOn>();

        lookOnCollider = findLookOn.GetComponent<Collider>();
      
    }


    public void LockOn(InputAction.CallbackContext context)
    {
        if (!context.performed || lockTaget || lockOut) return;


        if (!lookOnCollider.enabled && !getTarget /*&& !lockTaget && !lockOut*/)
        {
    

            lookOnCollider.enabled = true;

            lockTaget = true;


            StartCoroutine(ColliderFalse());
           
        }
        else if (lookOn.LockOnCineVir.enabled && getTarget)
        {
        

            lookOnCollider.enabled = false;


            lookOn.LockOnCineVir.enabled = false;

            getTarget = false;

            lockOut = true;

            OffLockOn();


            StartCoroutine(ColliderFalse());
            //Invoke("LockOutFalse",0.5f);
        }
    }

    /// <summary>
    /// ターゲットを除外し、追跡中フラグをfalseにするメソッドです。
    /// </summary>
    public void OffLockOn()
    { 
    lookOn.TargetTramsform.Remove(lookOn.TargetTramsform[0]);

        if (getTarget)
        { 
        getTarget = false;
        }

        if (lockOut)
        { 
        lockOut = false;
        }
    }

    private IEnumerator ColliderFalse()
    {
        yield return new WaitForSeconds(0.5f);

        lookOnCollider.enabled = false;
        lockTaget = false;
    }


}
