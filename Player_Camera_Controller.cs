
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̃J�����𐧌䂷��N���X�ł��B
/// </summary>
public class Player_Camera_Controller : MonoBehaviour
{

    [SerializeField]
    private LookOn lockOn; //���b�N�I���̊ԈႢ�ł���ˁH���������Ȃ�C�����肢���܂�
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
            Invoke("ColliderFalse", 0.5f); //�}�W�b�N�i���o�[�����I�ϐ����@OR�@�萔���@OR�@�R�����g�c���Ă��Ă�������
           
        }
        else if (lockOn.LockOnCineVir.enabled && getTarget && lockTarget == false && lockOut == false)
        {
            lockOnCollider.enabled = false;

            lockOn.TargetTramsform.Clear(); //�Â�Ԉ���Ă���BTransform��������

            lockOn.LockOnCineVir.enabled = false;

            getTarget = false;

            lockOut = true;

            Invoke("LockOutFalse",0.5f); //�}�W�b�N�i���o�[�����I�ϐ����@OR�@�萔���@OR�@�R�����g�c���Ă��Ă�������
        }
    }

    /// <summary>
    /// ���������Ă��������B���\�b�h�������ł͂킩��Ȃ��B����������ƕ�����₷�����O�ɂ��Ă��������B
    /// �Ⴆ�΁uDeactivateCollider�v�Ȃ�
    /// </summary>
    private void ColliderFalse()
    {
        lockOnCollider.enabled = false;
        lockTarget = false;
    }

    /// <summary>
    /// ���������Ă��������B���\�b�h�������ł͂킩��Ȃ��B����������ƕ�����₷�����O�ɂ��Ă��������B
    /// </summary>
    private void LockOutFalse()
    {
        lockOut = false;
    }
}
