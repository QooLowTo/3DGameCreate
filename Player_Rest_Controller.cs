using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動を制御するクラスです。ですよね？なぜRest？？
/// Moveとかの方がいいのでは？
/// </summary>
public class Player_Rest_Controller : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;//キャラクターのスピード
    [SerializeField]
    private float jumpForce = 5f;//キャラクターのジャンプ力
    [SerializeField] 
    private float animSpeed = 1.5f;//アニメーションの更新速度
    [SerializeField] 
    private float rotateSpeed = 1200;//キャラクターの向きの回転速度
    [SerializeField] 
    private float doPower = 20f;//DoTweenの移動量
    [SerializeField] 
    private float doTime = 0.5f;//DoTweenの移動時間

    [SerializeField] 
    private int playerHP = 100;

    [SerializeField]
    private CinemachineInputAxisController cameraAxis;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGM;

    private PlayerInput playerInput;//インプットシステム
    private CharacterController charaCon;//キャラクターコントローラー
    private Animator animCon;//アニメーター

    private bool gameStart = false;//ゲームを開始できるようにする。

    private bool groundedPlayer = false;//地面にいるかどうか判定用

    private bool isJumping = false;//ジャンプ中判定

    private bool starting = true;//開幕ジャンプ中アニメ封じ用

    private float gravityValue = -9.81f;

    Vector2 moveInput;//移動のベクトル

    private float moveInputAbs_x; //説明書いて

    private float moveInputAbs_y; //説明書いて

    private float moveInputAbs; //zではない？

    [SerializeField] private float playerVelocity;//縦方向のベクトル

    [SerializeField] private float rayLength = 1f;

    [SerializeField] private float rayOffset;

    [SerializeField]
    LayerMask groundLayers = default;

    private float coolTime = 0.3f;//コルーチンの間隔

    //---プロパティ---//
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

        void ChangeDirection(Vector3 MoveVelocity)//回転処理
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


    //----------------------ジャンプアニメーションイベントで制御----------------------//
  

    public void isJumpFalse()//アニメーションイベントで制御
    {
        animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    //--------------------------------------------------------------------------------//

    public void Landing()//着地アニメーションイベントで制御
    {
        isJumping = false;
       
        animCon.SetBool("Landing", false);
    }

    public void GameStart()//シグナルで制御
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
