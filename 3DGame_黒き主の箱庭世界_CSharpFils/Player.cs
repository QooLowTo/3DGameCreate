using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;

public class Player : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    protected float playerSpeed = 5f;//�L�����N�^�[�̃X�s�[�h

    [SerializeField, Header("�v���C���[�̃W�����v��")]
    protected float jumpForse = 5f;//�L�����N�^�[�̃W�����v��
    protected float jumpActioningForce = 1f;//����̃A�N�V�����ɂ��W�����v�␳��

    [SerializeField, Header("�L�����N�^�[�̌����̉�]���x")]
    protected float rotateSpeed = 1200;//�L�����N�^�[�̌����̉�]���x

    [SerializeField]
    protected float animSpeed = 1.5f;//�A�j���[�V�����̍X�V���x

    protected Vector2 moveInput;//�ړ��̃x�N�g��

    protected float moveInputAbs_x;

    protected float moveInputAbs_y;

    protected float moveInputAbs;

    protected float gravityValue = -9.81f;

    [SerializeField] 
    protected float playerVelocity;//�c�����̃x�N�g��

    [SerializeField] 
    protected float rayLength = 1f;

    [SerializeField] 
    protected float rayOffset;

    protected int attackCombo = 0;//�U����

    [SerializeField, Header("�X�^�[�g�|�W�V����")]
    protected Vector3 startPos;
    [SerializeField, Header("�X�^�[�g���̌���")]
    protected Quaternion startRotate;

    [SerializeField]
    protected CinemachineInputAxisController cameraAxis;

    [SerializeField]
    protected PlayerTransformData playerTransformData;
    [SerializeField]
    protected FlagManagementData flagmentData;

    protected Player_Status_Controller plasta;

    protected SoundManager soundManager;

    [SerializeField]
    protected GameObject findSoundManager;

    protected PlayerInput plaIn;//�C���v�b�g�V�X�e��

    protected CharacterController charaCon;//�L�����N�^�[�R���g���[���[

    protected Animator animCon;//�A�j���[�^�[

    [SerializeField,Header("�Q�[���J�n")]
    protected bool gameStart = false;//�v���C���[�𑀍�ł���悤�ɂ���B

    [SerializeField, Header("�Q�[���I�[�o�[")]
    protected bool gameOver = false;//�Q�[���I�[�o�[����

    protected bool starting = true;//�J���W�����v���A�j�������p

    private bool isChanegeAction = false;//�s��������

    protected bool isJumpping = false;//�W�����v������

    private bool onJumpping = false;//��i�W�����v������

    protected bool falling = false;//�󒆔���

    protected bool landing = true;//���n����

    protected bool attacking = false;//�U��������

    protected bool jumpAttackingFall = false;//�󒆍U��������

    protected bool avoidancing = false;//��𒆔���

    protected bool hitting = false;//�q�b�g������

    protected bool isDown = false;//�_�E��������

    [SerializeField, Header("�n�ʔ��胊�X�g")]
    protected List<bool> groundPlayerList = new List<bool>();

    [SerializeField, Header("�n�ʃ��C���[���X�g")]
    protected List<LayerMask> groundLayerList = new List<LayerMask>();

    private float cool = 0.3f;//�R���[�`���̊Ԋu


    //---�v���p�e�B---//
    public bool GameStart { get => gameStart; set => gameStart = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public CharacterController CharaCon { get => charaCon; set => charaCon = value; }
    public Animator AnimCon { get => animCon; set => animCon = value; }
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }
    public PlayerInput PlaIn { get => plaIn; set => plaIn = value; }
 





    //private void FixedUpdate()
    //{
    //    Debug.Log("s");

    //    //animCon.speed = animSpeed;

    //    if (landing && OnGroundLayer(false)){

    //        if (starting || hitting) return;

    //        Fall();
    //    }

    //    if (!landing && OnGroundLayer(true))
    //    {

    //        landing = true;

    //        if (jumpAttackingFall)
    //        {

    //            AttackGrounded();

    //        }
    //        else
    //        {

    //            Grounded();
    //        }
    //    }
    //}

    protected void StartPlayerSet()
    {
        plaIn.enabled = false;

        charaCon.enabled = false;

        Debug.Log("��");

        if (flagmentData.PositionLoad)
        {

            transform.position = playerTransformData.LoadTransform;

            flagmentData.PositionLoad = false;

        }
        else
        {

            transform.position = startPos;

        }



        if (flagmentData.RotateLoad)
        {

            transform.rotation = playerTransformData.LoadRotate;

            flagmentData.RotateLoad = false;

        }
        else
        {

            transform.rotation = startRotate;
        }

        animCon.speed = animSpeed;

    }

    protected IEnumerator Starting()
    {
        yield return new WaitForSeconds(cool);
        starting = false;

    }

    protected void PlayerMoveInputControl()
    {

        moveInputAbs_x = Mathf.Abs(moveInput.x);

        moveInputAbs_y = Mathf.Abs(moveInput.y);

        moveInputAbs = moveInputAbs_x + moveInputAbs_y;

        animCon.SetFloat("MoveInput", moveInputAbs);


       
        //animCon.SetFloat("MoveInput", 0);

    }

    protected void PlayerMoveControl()
    {
        var CameraFowerd = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        var MoveVelocity = CameraFowerd * new Vector3(moveInput.x, 0, moveInput.y).normalized;

        var MoveDelta = MoveVelocity * Time.deltaTime;

        ChangeDirection(MoveVelocity);
        charaCon.Move(MoveDelta * playerSpeed);
    }

    protected void PlayerGravityControl()
    {
        var gravityVelocity = new Vector3(0, playerVelocity, 0);

        var gravityDelta = gravityVelocity * Time.deltaTime;

        charaCon.Move(gravityDelta);
    }

    protected void Fall()//��������擾��
    {
        if (falling) return;

        if (!jumpAttackingFall)
        {
            animCon.SetBool("Falling", true);
        }

        //Invoke("Grounded", 0.7f);
        landing = false;

        falling = true;
    }

    /// <summary>
    /// �v���C���[�̉�]�������s�����\�b�h�ł��B
    /// </summary>
    /// <param name="MoveVelocity"></param>
    protected void ChangeDirection(Vector3 MoveVelocity)
    {
        if (moveInput != Vector2.zero)
        {

            Quaternion q = Quaternion.LookRotation(MoveVelocity);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
        }
    }
    /// <summary>
    /// �v���C���[�ɂ�����d�͕��ׂ����Z���郁�\�b�h�ł��B
    /// </summary>
    protected void GravityLoad()
    {
        if (OnGroundLayer(false) && playerVelocity > -10f)
        {
            //if (playerVelocity <= -10f) return;

            playerVelocity += (gravityValue * jumpActioningForce) * Time.deltaTime;
        }
    }
    /// <summary>
    /// groundPlayerList�̃J�E���g��������Ƃ��A�n�ʃ��C���[���J��Ԃ����肷�郁�\�b�h�ł��B
    /// </summary>
    protected void CheckGroundPlayerList()
    {
        for (int i = 0; i < groundPlayerList.Count; i++)
        {
            groundPlayerList[i] = CheckGroundLayer(groundLayerList[i]);
        }
    }
    /// <summary>
    /// ray����Ƀv���C���[���n�ʂɒ����Ă��邩�A���Ȃ����𔻒肷�郁�\�b�h�ł��B
    /// </summary>
    /// <param name="groundLayer"></param>
    /// <returns></returns>
    protected bool CheckGroundLayer(LayerMask groundLayer)
    {
        // �������̏����ʒu�Ǝp��
        // �኱�g�̂ɂ߂荞�܂����ʒu���甭�˂��Ȃ��Ɛ���������ł��Ȃ���������
        var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

        // Raycast��hit���邩�ǂ����Ŕ���
        // ���C���̎w���Y�ꂸ��
        return Physics.Raycast(ray, rayLength, groundLayer);
    }

    /// <summary>
    /// �v���C���[���n�ʂ̏�ɂ��邩�A�������́A���Ȃ����̔���̌��ʂ�Ԃ��܂��B������true�ɂ���Ɓu�n�ʂ̏�ɂ��邩�v�Afalse�ɂ���Ɓu�n�ʂ̏�ɂ��Ȃ����v�����ꂼ�ꔻ�肷��悤�ɂȂ�܂��B
    /// </summary>
    /// <param name="onGround"></param>
    /// <returns></returns>
    protected bool OnGroundLayer(bool onGround)
    {
        bool result = false;

        if (onGround)
        {
            int i = 0;

            while (i < groundPlayerList.Count)
            {
               
                if (groundPlayerList[i])
                {
                    result = true;
                }

                i++;
            }

            //if (groundPlayerList[0] || groundPlayerList[1] || groundPlayerList[2])
            //{
            //    result = true;
            //}
        }
        else if (!onGround)
        {
            int i = 0;

            int groundPlayerCount = 0;

            while (i < groundPlayerList.Count)
            {

                if (!groundPlayerList[i])
                {
                    groundPlayerCount++;
                }

                i++;
            }

            if (groundPlayerCount == groundPlayerList.Count)
            { 
             result = true;
            }

            //if (!groundPlayerList[0] && !groundPlayerList[1] && !groundPlayerList[2])
            //{
            //    result = true;
            //}
        }

        return result;
    }

    protected void Grounded()//���n���Ɏg�p
    {

        animCon.SetBool("Falling", false);

        playerVelocity = 0;

        animCon.SetBool("Landing", true);

    }

    protected void AttackGrounded()
    {
        attacking = true;

        animCon.SetBool("JumpAttackFall", false);

        animCon.SetBool("JumpAttack", true);

        jumpAttackingFall = false;

        playerVelocity = 0;

        jumpActioningForce = 1;

    }

    protected void LandingSound()
    {
        soundManager.OneShot_Player_Sound(4);//�W�����v���n�T�E���h
    }

    private void OnDrawGizmos()
    {
        // �ڒn���莞�͗΁A�󒆂ɂ���Ƃ��͐Ԃɂ���
        Gizmos.color = OnGroundLayer(true) ? Color.green : Color.red;

        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);

    }

   

    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed /*|| isJumpping */|| attacking || avoidancing || hitting || isDown /*|| OnGroundLayer(false)*/) return;

        Debug.Log("s");

      

        if (!isJumpping)
        {
            Debug.Log("s1");

            animCon.SetBool("JumpAnim", true);

            //animCon.SetBool("Landing", true);
            playerVelocity += jumpForse;

            soundManager.OneShot_Player_Sound(2);//�W�����v�T�E���h

            isJumpping = true;
        }
        else if (isJumpping && onJumpping)
        {
            Debug.Log("s2");

            animCon.SetBool("JumpAnim", false);

            animCon.SetBool("OnJump", true);

            playerVelocity += jumpForse % 80;

            soundManager.OneShot_Player_Sound(3);//��i�W�����v�T�E���h

            onJumpping = false;
        }

       


    }



    /// <summary>
    /// �v���C���[�̑������o�����\�b�h�B�A�j���[�V�����C�x���g�Ŏg�p
    /// </summary>
    /// <param name="moveNum"></param>
    public void MoveSound(int moveNum)
    {

        switch (moveNum)
        {

            case 1:

                soundManager.OneShot_Player_Move_Sound(0);


                break;

            case 2:

                soundManager.OneShot_Player_Move_Sound(1);


                break;
        }

    }

    //----------------------�W�����v�A�j���[�V�����C�x���g�Ő���----------------------//
    public void OnJumpTrue()//�A�j���[�V�����C�x���g�Ő���
    {
        onJumpping = true;
    }

    public void isJumpFalse()//�A�j���[�V�����C�x���g�Ő���
    {
        isChanegeAction = false;
        animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    public void OnJumpFalse()//�A�j���[�V�����C�x���g�Ő���
    {
        isChanegeAction = false;
        animCon.SetBool("OnJump", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------���n�A�j���[�V�����C�x���g�Ő���----------------------//
    public void JumpBoolFalse()
    {
        isJumpping = false;
        onJumpping = false;
    }

    //public void LandingSound()
    //{
    //    gameManager.OneShotPlayerSound(4);
    //}
    public void Landing()
    {
        //isJumpping = false;
        //onJumpping = false;

        if (playerVelocity != 0)
        {//playerVelocity�̏�����

            playerVelocity = 0;
            Debug.Log("s0");
        }

        falling = false;

        animCon.SetBool("Landing", false);
    }

    //--------------------------------------------------------------------------------//
}
