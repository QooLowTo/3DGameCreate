using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.UIElements;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using Unity.Cinemachine;


/// <summary>
/// �o�g�����̃v���C���[�𐧌䂷��N���X�ł��B
/// </summary>
public class Player_Battle_Controller : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f; //�L�����N�^�[�̃X�s�[�h
    private float returnSpeed = 5f; //�X�s�[�h�̏����l

    [SerializeField] 
    private float jumpForse = 5f;//�L�����N�^�[�̃W�����v��

    [SerializeField] 
    private float animSpeed = 1.5f;//�A�j���[�V�����̍X�V���x

    [SerializeField]
    private float rotateSpeed = 1200;//�L�����N�^�[�̌����̉�]���x

    [SerializeField]
    private int doDashFastSpeed = 0;//�_�b�V���̏����x

    [SerializeField] 
    private float doAvoidancePower = 20f;//DoTween�̈ړ���

    [SerializeField] 
    private float doTime = 0.5f; //DoTween�̈ړ�����


    [SerializeField]
    private GameObject myWeapon;//�v���C���[�̕���

    [SerializeField]
    private GameObject weaponEffect;//�g���C��

    [SerializeField] 
    private int attackCombo = 0;//�U����


    [SerializeField] 
    private Player_Status_Controller playerSCon;// �v���C���[�̃X�e�[�^�X�X�N���v�g

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGM;

    [SerializeField]
    private CinemachineInputAxisController cameraAxis;


    private PlayerInput playerInput;//�C���v�b�g�V�X�e��
    private CharacterController charaCon;//�L�����N�^�[�R���g���[���[
    private Animator animCon;//�A�j���[�^�[
    private RigBuilder rig;//���O�r���_�[(�_�b�V���A�j���[�V�����p)
    private InputAction DashInput;
 
    private bool groundedPlayer = false;//�n�ʂɂ��邩�ǂ�������p

    private bool isMoving = false;//����������

    private bool isDashing = false;//�_�b�V��������

    private bool isJumping = false;//�W�����v������

    private bool onJumping = false;//��i�W�����v������

    private bool isAttacking = false;//�U��������

    private bool stopCombo = false;//�R���{�����p

    private bool isAttackingWLockOn = false;//���b�N�I��������

    private bool isAvoiding = false;//��𒆔���

    private bool starting = true;//�J���W�����v���A�j�������p

    private bool isHitting = false;//�q�b�g������

    private bool isDown = false;//�_�E��������

    private bool gameStart = false;//�Q�[�����J�n�ł���悤�ɂ���B

    private bool gameOver = false;//�Q�[���I�[�o�[����

    private float gravityValue = -9.81f;

    Vector2 moveInput;//�ړ��̃x�N�g��

    private float moveInputAbs_x;

    private float moveInputAbs_y;

    private float moveInputAbs;

    [SerializeField] 
    private float playerVelocity; //�c�����̃x�N�g��

    [SerializeField] 
    private float rayLength = 1f;

    [SerializeField] 
    private float rayOffset;

    [SerializeField]
    LayerMask groundLayers = default;

    private float cool = 0.3f;//�R���[�`���̊Ԋu

 
    //---�v���p�e�B---//
    public CharacterController CharaCon { get => charaCon; set => charaCon = value; } 
    public Animator AnimCon { get => animCon; set => animCon = value; }
    public RigBuilder Rig { get => rig; set => rig = value; }
    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    public float RotateSpeed { get => rotateSpeed; set => rotateSpeed = value; }
    public bool IsDown { get => isDown; set => isDown = value; }
    public bool isAvoiding { get => isAvoiding; set => isAvoiding = value; }
    public bool isHitting { get => isHitting; set => isHitting = value; }
    public bool isAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsAttackingWLockOn { get => isAttackingWLockOn; set => isAttackingWLockOn = value; }
    public bool GroundedPlayer { get => groundedPlayer; set => groundedPlayer = value; }
    public bool StopCombo { get => stopCombo; set => stopCombo = value; }
    public bool GameStarting { get => gameStart; set => gameStart = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public int AttackCombo { get => attackCombo; set => attackCombo = value; }
    public float MoveInputAbs { get => moveInputAbs; set => moveInputAbs = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public GameObject MyWeapon { get => myWeapon; set => myWeapon = value; }
  
    public CinemachineInputAxisController CameraAxis { get => cameraAxis; set => cameraAxis = value; }
    public PlayerInput PlayerInput { get => playerInput; set => playerInput = value; }
    

    private void Start()
    {
        StartCoroutine(Starting());
      
    }
    IEnumerator Starting()
    {
        yield return new WaitForSeconds(cool);
        starting = false;
    }
    void Awake()
    {
        playerSCon = GetComponent<Player_Status_Controller>();
        playerInput = GetComponent<PlayerInput>();
        gameManager = findGM.GetComponent<GameManager>();
        charaCon = GetComponent<CharacterController>();
        cameraAxis.GetComponent<CinemachineInputAxisController>();
        animCon = GetComponent<Animator>();
        rig = GetComponent<RigBuilder>();

        DashInput = playerInput.actions["Dash"];
    }
   
    private void FixedUpdate()
    {
        if (gameOver || gameStart == false) return;

        groundedPlayer = CheckGrounded();
       
        moveInputAbs_x = Mathf.Abs(moveInput.x);

        moveInputAbs_y = Mathf.Abs(moveInput.y);

        moveInputAbs = moveInputAbs_x + moveInputAbs_y;

        if (isAttacking == false && isDown == false && isAvoiding == false && isHitting == false)
        {
         animCon.SetFloat("MoveInput", moveInputAbs);
        }
        else
        {
            animCon.SetFloat("MoveInput", 0);
        }  
        animCon.speed = animSpeed;


        animCon.SetInteger("AttackCombo",attackCombo);

        if (isAvoiding)
        {
            if (myWeapon.GetComponent<BoxCollider>().enabled == true)
            {
                OffCollider();
            }

            if (weaponEffect.GetComponent<TrailRenderer>().emitting == true)
            {
                OffEffect();
            }
        }

        if (isDown == true|| isHitting == true)
        {
            AllCancel();
        }
    }

    void Update()
    {
        if (gameOver || gameStart == false) return;

       

        var CameraFowerd = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            var MoveVelocity = CameraFowerd * new Vector3(moveInput.x, 0, moveInput.y).normalized;
          
            var MoveDelta =  MoveVelocity * Time.deltaTime;

        if (isAttacking == false && isDown == false && isAvoiding == false && isHitting == false)
        {
                ChangeDirection(MoveVelocity);
                charaCon.Move(MoveDelta * speed);     
        }

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

        var ispressed = DashInput.IsPressed();

        if (ispressed&&moveInputAbs != 0)
        {
            OnDash();
        }
        else
        {
            OffDash();
        }

        if ((moveInputAbs != 0) && groundedPlayer && isAttacking == false && isJumping == false&& isAvoiding == false && isDown == false)
        {
            rig.enabled = true;
        }
        else
        {
            rig.enabled = false;
        }
           

        if (isJumping == true&&onJumping == true)//��i�W�����v����
        {
            DoubleJump();
        }
        

        if (!groundedPlayer&&isHitting == false)
        {
            if (starting) return;
             playerVelocity += gravityValue * Time.deltaTime;
            animCon.SetBool("Falling", true);
            Invoke("Grounded",1f);
        }
        else
        {
            animCon.SetBool("Falling", false);
        }


        if (playerVelocity < 0)
        {
            //rayLength = 1;

            animCon.SetBool("JumpAnim", false);
            animCon.SetBool("OnJump", false);
            if (groundedPlayer)
            { 
               playerVelocity = 0f;
            }
        }

        if (isAttacking == true || isAvoiding == true)
        { 
            AttackingMoveLimit();
        }


        void AttackingMoveLimit()
        {
            var attackingMoveLimit = Vector3.down;
            var LimitDelta = attackingMoveLimit + (attackingMoveLimit * Time.deltaTime);
            charaCon.Move(LimitDelta * speed);
        }


            if (isAttacking == false)
            {
           
            stopCombo = false;
     
            }

        if (playerSCon.LivePlayerHP <= 0)
        {
            Die();
        }

     
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnDash()
    {
        if (isDashing == false&&isAttacking == false && isDown == false && isAvoiding == false && isHitting == false&&groundedPlayer)
        {
                animCon.SetBool("Dash", true);

                gameManager.Player_DashStart_Sound();

                DoForward(doDashFastSpeed);

                speed += speed % 50f; //�}�W�b�N�i���o�[�����I�ϐ����E�萔���E�R�����g�c���Ă�������

                isDashing = true;
            
        }
    }
    private void OffDash()
    {
        if (isDashing == false) return;

        animCon.SetBool("Dash", false);
        speed = returnSpeed;
        isDashing = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !groundedPlayer || isJumping == true || isAttacking == true || isAvoiding == true || isHitting == true || isDown == true) return; 
        animCon.SetBool("JumpAnim", true);
        animCon.SetBool("Landing", true);
        playerVelocity += jumpForse;
        gameManager.Player_Jump_Sound();
        isJumping = true;
    }

    public void DoubleJump()//��i�W�����v����
    {
        if (playerInput.actions["Jump"].triggered&&!groundedPlayer)
        {
            animCon.SetBool("JumpAnim", false);
            animCon.SetBool("OnJump", true);
            playerVelocity += jumpForse;
            gameManager.Player_DoubleJump_Sound();
            onJumping = false;
        }
    }

    private void Grounded()
    {
        if (!groundedPlayer)
        {
            animCon.SetBool("Landing", true);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
 
        if (isAttacking == false && isHitting == false && isDown == false && isAvoiding == false && groundedPlayer)
        {
            isAttacking = true;

            isAttackingWLockOn = true;

            animCon.SetBool("Attacking", true);//AnySatate�̐���p
        }

        if (stopCombo == false && isHitting == false && isDown == false && isAvoiding == false && groundedPlayer)
        {
            attackCombo++;
            stopCombo = true;

            if (attackCombo <= 1)
            { 
            animCon.SetTrigger("Attack");
            }
                
            
        }

    }

    public void Avoidance(InputAction.CallbackContext context)
    {
        if (!context.performed || isAvoiding || isHitting || isDown || !groundedPlayer) return;

        if  (moveInputAbs == 0 )
        {
            animCon.SetBool("BackAvoidance", true);

        }
        else if (isAttacking == true && moveInput.x >= 0.1f )
        {
            animCon.SetBool("RightAvoidance", true);

        }
        else if (isAttacking == true && moveInput.x <= -0.1f )
        {
            animCon.SetBool("LeftAvoidance", true);
           
        }

        else if (moveInputAbs >= 0.1f )
        {
            animCon.SetBool("ForwardAvoidance", true);

        } 

        OffAttacking(); 
        
        gameManager.Player_Avoidance_Sound();

        isAvoiding = true;
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

    //----------------------�ړ��A�j���[�V�����C�x���g�Ő���----------------------//

    public void MoveSound(int moveNum)
    {

        switch (moveNum)
        {

            case 1:

                gameManager.Player_Move_Sound_1();
             

                break;

            case 2:

                gameManager.Player_Move_Sound_2();
               

                break;
        }
            
    }


    //--------------------------------------------------------------------------------//

    //----------------------�A�^�b�N�A�j���[�V�����C�x���g�Ő���----------------------//
    public void OnCollider()
    {
        myWeapon.GetComponent<BoxCollider>().enabled = true;
        weaponEffect.GetComponent<TrailRenderer>().emitting = true;
        gameManager.Player_Swing_Sound();
    }
    public void OffCollider()
    {
        myWeapon.GetComponent<BoxCollider>().enabled = false; 
    }

    public void OffComboStop()
    { 
     stopCombo = false;
        
    }

    public void NextAttack()
    {
        animCon.SetTrigger("NextAttack");
    }

    public void OffAttackingLook()
    { 
    isAttackingWLockOn = false;
    }

    /// <summary>
    /// �A�b�^�N�Ɋւ���l�X�ȕϐ���false�܂��́A0�ɂ��܂��B�܂��A����̃R���C�_�[��false�łȂ���΃R���C�_�[��false�ɂ��܂��B
    /// </summary>
    public void OffAttacking()
    { 
        isAttacking = false;
        animCon.SetBool("Attacking", false);
        animCon.ResetTrigger("NextAttack");
        attackCombo = 0;
    }


    public void AttackForward(int DofowardPower)
    {
        DoForward(DofowardPower);
    }

    public void OffEffect()
    { 
      weaponEffect.GetComponent<TrailRenderer>().emitting = false;
    }

    //--------------------------------------------------------------------------------//

    private void DoForward(int DofowardPower)
    {
        DOTween.To(() => transform.position,
                    v =>
                    {
                        Vector3 velocity = (v - transform.position) * Time.deltaTime;
                        CharaCon.Move(velocity);
                    },
             transform.position + (transform.forward * DofowardPower), doTime);
    }

    //----------------------�W�����v�A�j���[�V�����C�x���g�Ő���----------------------//
    public void OnJumpTrue()//�A�j���[�V�����C�x���g�Ő���
    { 
    onJumping = true;
    }

    public void isJumpFalse()//�A�j���[�V�����C�x���g�Ő���
    { 
     animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    public void OnJumpFalse()//�A�j���[�V�����C�x���g�Ő���
    {
        animCon.SetBool("OnJump", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------���n�A�j���[�V�����C�x���g�Ő���----------------------//

    public void LandingSound()
    { 
     gameManager.Player_Landing_Sound();
    }
    public void Landing()
    {
       
        isJumping = false;
        onJumping = false;
        animCon.SetBool("Landing", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------����A�j���[�V�����C�x���g�Ő���----------------------//


    public void AvoidanceMove(int direction)
    {
        DOTween.To(() => transform.position,
                 v =>
                 {
                     Vector3 velocity = (v - transform.position) * Time.deltaTime;
                     charaCon.Move(velocity);
                 },
          transform.position + (direction * transform.forward * doAvoidancePower), doTime);
    }
    public void SideAvoidanceMove(int direction)
    {
        DOTween.To(() => transform.position,
                 v =>
                 {
                     Vector3 velocity = (v - transform.position) * Time.deltaTime;
                     charaCon.Move(velocity);
                 },
          transform.position + (direction * transform.right * doAvoidancePower), doTime);
    }
    public void CancelAvoidance(string AnimName)
    {
        animCon.SetBool(AnimName, false);
        isAvoiding = false;
    }

    public void OnHit()
    { 
        isHitting = true;

    }
    public void OffHit()//�A�j���[�V�����C�x���g�Ő���
    { 
        isHitting = false;

        if (groundedPlayer)
        {
            Landing();
        }
    }
    public void OnIsDown()//�A�j���[�V�����C�x���g�Ő���
    {
        isDown = true;

    }
    public void OffIsDown()//�A�j���[�V�����C�x���g�Ő���
    {
        isDown = false;
        AnimCon.SetBool("knockbacking", false);
    }

    public void AllCancel()
    {
        animCon.SetFloat("MoveInput", 0);

        OffCollider();
        OffComboStop();
        OffAttacking();
        OffEffect();
        isJumpFalse();
        OnJumpFalse();
        Landing();

        animCon.SetBool("Falling", false);

        isDashing = false;
        isJumping = false;
        onJumping = false;

        CancelAvoidance("BackAvoidance");
        CancelAvoidance("RightAvoidance");
        CancelAvoidance("LeftAvoidance");
        CancelAvoidance("ForwardAvoidance");
    }

    public void GameStart()//�V�O�i���Ő���
    { 
    gameStart = true;
    playerSCon.HealthBar.SetActive(true);
        cameraAxis.enabled = true;
    }

    private void Die()
    {
        AllCancel();
        rig.enabled = false;
        animCon.SetTrigger("Die");
        charaCon.enabled = false;
        gameManager.GameOver();
        gameOver = true;
    }

    
}
