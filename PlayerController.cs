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
/// バトル中のプレイヤーを制御するクラスです。
/// </summary>
public class Player_Battle_Controller : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;//キャラクターのスピード
    private float returnSpeed = 5f;

    [SerializeField] 
    private float jumpForse = 5f;//キャラクターのジャンプ力

    [SerializeField] 
    private float animSpeed = 1.5f;//アニメーションの更新速度

    [SerializeField]
    private float rotateSpeed = 1200;//キャラクターの向きの回転速度

    [SerializeField]
    private int doDashFastSpeed = 0;//ダッシュの初速度

    [SerializeField] 
    private float doAvoidancePower = 20f;//DoTweenの移動量

    [SerializeField] 
    private float doTime = 0.5f;//DoTweenの移動時間


    [SerializeField]
    private GameObject myWepon;//プレイヤーの武器

    [SerializeField]
    private GameObject weponEffect;//トレイル

    [SerializeField] 
    private int attackCombo = 0;//攻撃回数


    [SerializeField] 
    private Player_Status_Controller plasta;// プレイヤーのステータススクリプト

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private GameObject FindGM;

    [SerializeField]
    private CinemachineInputAxisController cameraAxis;


    private PlayerInput plaIn;//インプットシステム
    private CharacterController charaCon;//キャラクターコントローラー
    private Animator animCon;//アニメーター
    private RigBuilder rig;//リグビルダー(ダッシュアニメーション用)
    private InputAction DashInput;
   

 
    private bool groundedPlayer = false;//地面にいるかどうか判定用

    private bool isMove = false;//歩き中判定

    private bool isDash = false;//ダッシュ中判定

    private bool isJumpping = false;//ジャンプ中判定

    private bool onJumpping = false;//二段ジャンプ中判定

    private bool attacking = false;//攻撃中判定

    private bool stopCombo = false;//コンボ封じ用

    private bool attackingLook = false;//ロックオン中判定

    private bool avoidancing = false;//回避中判定

    private bool starting = true;//開幕ジャンプ中アニメ封じ用

    private bool hitting = false;//ヒット中判定

    private bool isDown = false;//ダウン中判定

    private bool gameStart = false;//ゲームを開始できるようにする。

    private bool gameOver = false;//ゲームオーバー判定

    private float gravityValue = -9.81f;

    Vector2 moveInput;//移動のベクトル

    private float moveInputAbs_x;

    private float moveInputAbs_y;

    private float moveInputAbs;

    [SerializeField] 
    private float playerVelocity;//縦方向のベクトル

    [SerializeField] 
    private float rayLength = 1f;

    [SerializeField] 
    private float rayOffset;

    [SerializeField]
    LayerMask groundLayers = default;

    private float cool = 0.3f;//コルーチンの間隔

 
    //---プロパティ---//
    public CharacterController CharaCon { get => charaCon; set => charaCon = value; } 
    public Animator AnimCon { get => animCon; set => animCon = value; }
    public RigBuilder Rig { get => rig; set => rig = value; }
    public GameManager GameManager { get => GM; set => GM = value; }
    public float RotateSpeed { get => rotateSpeed; set => rotateSpeed = value; }
    public bool IsDown { get => isDown; set => isDown = value; }
    public bool Avoidancing { get => avoidancing; set => avoidancing = value; }
    public bool Hitting { get => hitting; set => hitting = value; }
    public bool Attacking { get => attacking; set => attacking = value; }
    public bool AttackingLook { get => attackingLook; set => attackingLook = value; }
    public bool GroundedPlayer { get => groundedPlayer; set => groundedPlayer = value; }
    public bool StopCombo { get => stopCombo; set => stopCombo = value; }
    public bool GameStarting { get => gameStart; set => gameStart = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public int AttackCombo { get => attackCombo; set => attackCombo = value; }
    public float MoveInputAbs { get => moveInputAbs; set => moveInputAbs = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public GameObject MyWepon { get => myWepon; set => myWepon = value; }
  
    public CinemachineInputAxisController CameraAxis { get => cameraAxis; set => cameraAxis = value; }
    public PlayerInput PlaIn { get => plaIn; set => plaIn = value; }
    



    // Start is called before the first frame update

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
        plasta = GetComponent<Player_Status_Controller>();
        plaIn = GetComponent<PlayerInput>();
        GM = FindGM.GetComponent<GameManager>();
        charaCon = GetComponent<CharacterController>();
        cameraAxis.GetComponent<CinemachineInputAxisController>();
        animCon = GetComponent<Animator>();
        rig = GetComponent<RigBuilder>();

        DashInput = plaIn.actions["Dash"];
    }
   
    private void FixedUpdate()
    {
        if (gameOver || gameStart == false) return;

        groundedPlayer = CheckGrounded();
       
        moveInputAbs_x = Mathf.Abs(moveInput.x);

        moveInputAbs_y = Mathf.Abs(moveInput.y);

        moveInputAbs = moveInputAbs_x + moveInputAbs_y;

        if (attacking == false && isDown == false && avoidancing == false && hitting == false)
        {
         animCon.SetFloat("MoveInput", moveInputAbs);
        }
        else
        {
            animCon.SetFloat("MoveInput", 0);
        }  
        animCon.speed = animSpeed;


        animCon.SetInteger("AttackCombo",attackCombo);

        if (avoidancing)
        {
            if (myWepon.GetComponent<BoxCollider>().enabled == true)
            {
                OffCollider();
            }

            if (weponEffect.GetComponent<TrailRenderer>().emitting == true)
            {
                OffEffect();
            }
        }

        if (isDown == true||hitting == true)
        {
            AllCancel();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameOver || gameStart == false) return;

       

        var CameraFowerd = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            var MoveVelocity = CameraFowerd * new Vector3(moveInput.x, 0, moveInput.y).normalized;
          
            var MoveDelta =  MoveVelocity * Time.deltaTime;

        if (attacking == false && isDown == false && avoidancing == false && hitting == false)
        {
                ChangeDirection(MoveVelocity);
                charaCon.Move(MoveDelta * speed);     
        }

        var JumpVelocity = new Vector3(0, playerVelocity, 0);

            var JumpDelta = JumpVelocity * Time.deltaTime;

            charaCon.Move(JumpDelta);

        

        void ChangeDirection(Vector3 MoveVelocity)//回転処理
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

        if ((moveInputAbs != 0) && groundedPlayer && attacking == false && isJumpping == false&& avoidancing == false && isDown == false)
        {
            rig.enabled = true;
        }
        else
        {
            rig.enabled = false;
        }
           

        if (isJumpping == true&&onJumpping == true)//二段ジャンプ処理
        {
            DoubleJump();
        }
        

        if (!groundedPlayer&&hitting == false)
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

        if (attacking == true || avoidancing == true)
        { 
            AttackingMoveLimit();
        }


        void AttackingMoveLimit()
        {
            var attackingMoveLimit = Vector3.down;
            var LimitDelta = attackingMoveLimit + (attackingMoveLimit * Time.deltaTime);
            charaCon.Move(LimitDelta * speed);
        }


            if (attacking == false)
            {
           
            stopCombo = false;
     
            }

        if (plasta.LivePlayerHP <= 0)
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
        if (isDash == false&&attacking == false && isDown == false && avoidancing == false && hitting == false&&groundedPlayer)
        {
                animCon.SetBool("Dash", true);

                GM.Player_DashStart_Sound();

                DoForward(doDashFastSpeed);

                speed += speed % 50f;

                isDash = true;
            
        }
    }
    private void OffDash()
    {
        if (isDash == false) return;

        animCon.SetBool("Dash", false);
        speed = returnSpeed;
        isDash = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !groundedPlayer || isJumpping == true || attacking == true || avoidancing == true || hitting == true || isDown == true) return; 
        animCon.SetBool("JumpAnim", true);
        animCon.SetBool("Landing", true);
        playerVelocity += jumpForse;
        GM.Player_Jump_Sound();
        isJumpping = true;
    }

    public void DoubleJump()//二段ジャンプ処理
    {
        if (plaIn.actions["Jump"].triggered&&!groundedPlayer)
        {
            animCon.SetBool("JumpAnim", false);
            animCon.SetBool("OnJump", true);
            playerVelocity += jumpForse;
            GM.Player_DoubleJump_Sound();
            onJumpping = false;
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
 
        if (attacking == false && hitting == false && isDown == false && avoidancing == false && groundedPlayer)
        {
            attacking = true;

            attackingLook = true;

            animCon.SetBool("Attacking", true);//AnySatateの制御用
        }

        if (stopCombo == false && hitting == false && isDown == false && avoidancing == false && groundedPlayer)
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
        if (!context.performed || avoidancing || hitting || isDown || !groundedPlayer) return;

        if  (moveInputAbs == 0 )
        {
            animCon.SetBool("BackAvoidance", true);

        }
        else if (attacking == true && moveInput.x >= 0.1f )
        {
            animCon.SetBool("RightAvoidance", true);

        }
        else if (attacking == true && moveInput.x <= -0.1f )
        {
            animCon.SetBool("LeftAvoidance", true);
           
        }

        else if (moveInputAbs >= 0.1f )
        {
            animCon.SetBool("ForwardAvoidance", true);

        } 

        OffAttacking(); 
        
        GM.Player_Avoidance_Sound();

        avoidancing = true;
    }

    private bool CheckGrounded()
    {
        // 放つ光線の初期位置と姿勢
        // 若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
        var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

        // Raycastがhitするかどうかで判定
        // レイヤの指定を忘れずに
        return Physics.Raycast(ray, rayLength, groundLayers);
    }

    private void OnDrawGizmos()
    {
        // 接地判定時は緑、空中にいるときは赤にする
        Gizmos.color = groundedPlayer ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }

    //----------------------移動アニメーションイベントで制御----------------------//

    public void MoveSound(int moveNum)
    {

        switch (moveNum)
        {

            case 1:

                GM.Player_Move_Sound_1();
             

                break;

            case 2:

                GM.Player_Move_Sound_2();
               

                break;
        }
            
    }


    //--------------------------------------------------------------------------------//

    //----------------------アタックアニメーションイベントで制御----------------------//
    public void OnCollider()
    {
        myWepon.GetComponent<BoxCollider>().enabled = true;
        weponEffect.GetComponent<TrailRenderer>().emitting = true;
        GM.Player_Swing_Sound();
    }
    public void OffCollider()
    {
        myWepon.GetComponent<BoxCollider>().enabled = false; 
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
    attackingLook = false;
    }

    /// <summary>
    /// アッタクに関する様々な変数をfalseまたは、0にします。また、武器のコライダーがfalseでなければコライダーをfalseにします。
    /// </summary>
    public void OffAttacking()
    { 
        attacking = false;
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
      weponEffect.GetComponent<TrailRenderer>().emitting = false;
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

    //----------------------ジャンプアニメーションイベントで制御----------------------//
    public void OnJumpTrue()//アニメーションイベントで制御
    { 
    onJumpping = true;
    }

    public void isJumpFalse()//アニメーションイベントで制御
    { 
     animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    public void OnJumpFalse()//アニメーションイベントで制御
    {
        animCon.SetBool("OnJump", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------着地アニメーションイベントで制御----------------------//

    public void LandingSound()
    { 
     GM.Player_Landing_Sound();
    }
    public void Landing()
    {
       
        isJumpping = false;
        onJumpping = false;
        animCon.SetBool("Landing", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------回避アニメーションイベントで制御----------------------//


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
        avoidancing = false;
    }

    public void OnHit()
    { 
        hitting = true;

    }
    public void OffHit()//アニメーションイベントで制御
    { 
    hitting = false;

        if (groundedPlayer)
        {
            Landing();
        }
    }
    public void OnisDown()//アニメーションイベントで制御
    {
        isDown = true;

    }
    public void OffisDown()//アニメーションイベントで制御
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

        isDash = false;
        isJumpping = false;
        onJumpping = false;

        CancelAvoidance("BackAvoidance");
        CancelAvoidance("RightAvoidance");
        CancelAvoidance("LeftAvoidance");
        CancelAvoidance("ForwardAvoidance");
    }

    public void GameStart()//シグナルで制御
    { 
    gameStart = true;
    plasta.HealthBar.SetActive(true);
        cameraAxis.enabled = true;
    }

    private void Die()
    {
        AllCancel();
        rig.enabled = false;
        animCon.SetTrigger("Die");
        charaCon.enabled = false;
        GM.GameOver();
        gameOver = true;
    }

    
}
