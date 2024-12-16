
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーのカメラを制御するクラスです。
/// </summary>
public class Player_Camera_Controller : MonoBehaviour
{

    [SerializeField]
    private LookOn lockOn; //ロックオンの間違いですよね？もしそうなら修正お願いします
    [SerializeField]
    private Collider lockOnCollider;
    [SerializeField]
    private GameObject findLockOn;

    private float distanceToTarget;

    private GameObject target;

    bool getTarget = false;

    bool lockTarget = false;

    [SerializeField]
    bool lockOut = false;

    public GameObject Target { get => target; set => target = value; }
    public bool GetTarget { get => getTarget; set => getTarget = value; }

    void Start()
    {

        lockOn = findLockOn.GetComponent<LookOn>();
        lockOnCollider = findLockOn.GetComponent<Collider>();
      
    }
   

    public void LockOn(InputAction.CallbackContext context)
    {
        
        if (lockOnCollider.enabled == false && getTarget == false && lockTarget == false && lockOut == false)
        { 
            lockOnCollider.enabled = true;
            lockTarget = true;
            Invoke("ColliderFalse", 0.5f); //マジックナンバー発見！変数化　OR　定数化　OR　コメント残してしてください
           
        }
        else if (lockOn.LockOnCineVir.enabled && getTarget && lockTarget == false && lockOut == false)
        {
            lockOnCollider.enabled = false;

            lockOn.TargetTramsform.Clear(); //つづり間違っている。Transformが正しい

            lockOn.LockOnCineVir.enabled = false;

            getTarget = false;

            lockOut = true;

            Invoke("LockOutFalse",0.5f); //マジックナンバー発見！変数化　OR　定数化　OR　コメント残してしてください
        }
    }

    /// <summary>
    /// 説明書いてください。メソッド名だけではわからない。もうちょっと分かりやすい名前にしてください。
    /// 例えば「DeactivateCollider」など
    /// </summary>
    private void ColliderFalse()
    {
        lockOnCollider.enabled = false;
        lockTarget = false;
    }

    /// <summary>
    /// 説明書いてください。メソッド名だけではわからない。もうちょっと分かりやすい名前にしてください。
    /// </summary>
    private void LockOutFalse()
    {
        lockOut = false;
    }
}
