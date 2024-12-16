using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̈ړ��𐧌䂷��N���X�ł��B�ł���ˁH�Ȃ�Rest�H�H
/// Move�Ƃ��̕��������̂ł́H
/// </summary>
public class Player_Rest_Controller : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;//�L�����N�^�[�̃X�s�[�h
    [SerializeField]
    private float jumpForce = 5f;//�L�����N�^�[�̃W�����v��
    [SerializeField] 
    private float animSpeed = 1.5f;//�A�j���[�V�����̍X�V���x
    [SerializeField] 
    private float rotateSpeed = 1200;//�L�����N�^�[�̌����̉�]���x
    [SerializeField] 
    private float doPower = 20f;//DoTween�̈ړ���
    [SerializeField] 
    private float doTime = 0.5f;//DoTween�̈ړ�����

    [SerializeField] 
    private int playerHP = 100;

    [SerializeField]
    private CinemachineInputAxisController cameraAxis;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGM;

    private PlayerInput playerInput;//�C���v�b�g�V�X�e��
    private CharacterController charaCon;//�L�����N�^�[�R���g���[���[
    private Animator animCon;//�A�j���[�^�[

    private bool gameStart = false;//�Q�[�����J�n�ł���悤�ɂ���B

    private bool groundedPlayer = false;//�n�ʂɂ��邩�ǂ�������p

    private bool isJumping = false;//�W�����v������

    private bool starting = true;//�J���W�����v���A�j�������p

    private float gravityValue = -9.81f;

    Vector2 moveInput;//�ړ��̃x�N�g��

    private float moveInputAbs_x; //����������

    private float moveInputAbs_y; //����������

    private float moveInputAbs; //z�ł͂Ȃ��H

    [SerializeField] private float playerVelocity;//�c�����̃x�N�g��

    [SerializeField] private float rayLength = 1f;

    [SerializeField] private float rayOffset;

    [SerializeField]
    LayerMask groundLayers = default;

    private float coolTime = 0.3f;//�R���[�`���̊Ԋu

    //---�v���p�e�B---//
    public PlayerInput Player_Input { get => playerInput; set => playerInput = value; }

    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !groundedPlayer || isJumping == true ) return;
        gameManager.Player_Jump_Sound();
        animCon.SetBool("JumpAnim", true);
        animCon.SetBool("Landing", true);
        playerVelocity += jumpForce;
        isJumping = true;
    }

    private bool CheckGrounded()
    {
        // �������̏����ʒu�Ǝp��
        // �኱�g�̂ɂ߂荞�܂����ʒu���甭�˂��Ȃ��Ɛ���������ł��Ȃ���������
        var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

        // Raycast��hit���邩�ǂ����Ŕ���
        // ���C���̎w���Y�ꂸ��
        return Physics.Raycast(ray, rayLength, groundLayers);
    }



    private void OnDrawGizmos()
    {
        // �ڒn���莞�͗΁A�󒆂ɂ���Ƃ��͐Ԃɂ���
        Gizmos.color = groundedPlayer ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }
    private void Start()
    {
        StartCoroutine(Starting());
    }
    IEnumerator Starting()
    {
        yield return new WaitForSeconds(coolTime);
        starting = false;
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        charaCon = GetComponent<CharacterController>();
        animCon = GetComponent<Animator>();
        gameManager = findGM.GetComponent<GameManager>();
        cameraAxis.GetComponent<CinemachineInputAxisController>();

    }
    private void FixedUpdate()
    {
        if (gameStart == false) return;

        groundedPlayer = CheckGrounded();

        moveInputAbs_x = Mathf.Abs(moveInput.x);

        moveInputAbs_y = Mathf.Abs(moveInput.y);

        moveInputAbs = moveInputAbs_x + moveInputAbs_y;

        animCon.SetFloat("MoveInput", moveInputAbs);

        animCon.speed = animSpeed;

       
    }
    // Update is called once per frame
    void Update()
    {
        if (gameStart == false) return;

        var CameraFowerd = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        var MoveVelocity = CameraFowerd * new Vector3(moveInput.x, 0, moveInput.y).normalized;

        var MoveDelta = MoveVelocity * Time.deltaTime;

        ChangeDirection(MoveVelocity);
        charaCon.Move(MoveDelta * speed);

        var JumpVelocity = new Vector3(0, playerVelocity, 0);

        var JumpDelta = JumpVelocity * Time.deltaTime;

        charaCon.Move(JumpDelta);

        void ChangeDirection(Vector3 MoveVelocity)//��]����
        {
            if (moveInput != Vector2.zero)
            {
                Quaternion q = Quaternion.LookRotation(MoveVelocity);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
            }
        }

        if (!groundedPlayer)
        { 
             playerVelocity += gravityValue * Time.deltaTime;
             if (starting) return;
           
            animCon.SetBool("Falling", true);
            Invoke("Grounded", 1f);

        }
        else
        {
            animCon.SetBool("Falling", false);
        }

        if (playerVelocity < 0)
        {
         
            animCon.SetBool("JumpAnim", false);

            if (groundedPlayer)
            {
                playerVelocity = 0f;
            }

        }
    }


    //----------------------�W�����v�A�j���[�V�����C�x���g�Ő���----------------------//
  

    public void isJumpFalse()//�A�j���[�V�����C�x���g�Ő���
    {
        animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    //--------------------------------------------------------------------------------//

    public void Landing()//���n�A�j���[�V�����C�x���g�Ő���
    {
        isJumping = false;
       
        animCon.SetBool("Landing", false);
    }

    public void GameStart()//�V�O�i���Ő���
    {
        gameStart = true;
        cameraAxis.enabled = true;
    }

    private void Grounded()
    {
        if (!groundedPlayer)
        {
            animCon.SetBool("Landing", true);
        }
    }

    public void LandingSound()
    {
        gameManager.Player_Landing_Sound();
    }
}
