
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Camera_Controller : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        lookOn = findLookOn.GetComponent<LockOn>();

        lookOnCollider = findLookOn.GetComponent<Collider>();
      
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

   

    public void LockOn(InputAction.CallbackContext context)
    {
        if (!context.performed || lockTaget || lockOut) return;


        if (!lookOnCollider.enabled && !getTarget /*&& !lockTaget && !lockOut*/)
        {
    

            lookOnCollider.enabled = true;

            lockTaget = true;


            StartCoroutine(ColliderFalse());

            //Invoke("ColliderFalse", 0.5f);
           
        }
        else if (lookOn.LockOnCineVir.enabled && getTarget/* && !lockTaget && !lockOut*/)
        {
        

            lookOnCollider.enabled = false;

            //lookOn.TargetTramsform.Clear();

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

    //private void LockOutFalse()
    //{
    //    lockOut = false;

    //    Debug.Log("ee");
    //}
}
