
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Camera_Controller : MonoBehaviour
{

    [SerializeField]
    private LookOn lookOn;
    [SerializeField]
    private Collider lookOnCollider;
    [SerializeField]
    private GameObject findLookOn;

    private float distanceToTarget;

    private GameObject target;

    bool getTarget = false;

    bool lockTaget = false;

    [SerializeField]
    bool lockOut = false;

    public GameObject Target { get => target; set => target = value; }
    public bool GetTarget { get => getTarget; set => getTarget = value; }

    // Start is called before the first frame update
    void Start()
    {

        lookOn = findLookOn.GetComponent<LookOn>();
        lookOnCollider = findLookOn.GetComponent<Collider>();
      
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

   

    public void LockOn(InputAction.CallbackContext context)
    {
        
        if (lookOnCollider.enabled == false && getTarget == false && lockTaget == false && lockOut == false)
        { 
            lookOnCollider.enabled = true;
            lockTaget = true;
            Invoke("ColliderFalse", 0.5f);
           
        }
        else if (lookOn.LockOnCineVir.enabled && getTarget && lockTaget == false && lockOut == false)
        {
            lookOnCollider.enabled = false;

            lookOn.TargetTramsform.Clear();

            lookOn.LockOnCineVir.enabled = false;

            getTarget = false;

            lockOut = true;

            Invoke("LockOutFalse",0.5f);
        }
    }

    private void ColliderFalse()
    {
        lookOnCollider.enabled = false;
        lockTaget = false;
    }

    private void LockOutFalse()
    {
        lockOut = false;
    }
}
