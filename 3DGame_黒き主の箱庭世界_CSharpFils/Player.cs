using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;
/// <summary>
/// プレイヤーの動きを制御するクラスの親クラスです。
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの移動速度")]
    protected float playerSpeed = 5f;//キャラクターのスピード

    [SerializeField, Header("プレイヤーのジャンプ力")]
    protected float jumpForse = 5f;//キャラクターのジャンプ力
    protected float jumpActioningForce = 1f;//特定のアクションによるジャンプ補正力

    [SerializeField, Header("キャラクターの向きの回転速度")]
    protected float rotateSpeed = 1200;//キャラクターの向きの回転速度

    [SerializeField]
    protected float animSpeed = 1.5f;//アニメーションの更新速度

    protected Vector2 moveInput;//移動のベクトル

    protected float moveInputAbs_x;

    protected float moveInputAbs_y;

    protected float moveInputAbs;

    protected float gravityValue = -9.81f;

    [SerializeField] 
    protected float playerVelocity;//縦方向のベクトル

    [SerializeField] 
    protected float rayLength = 1f;

    [SerializeField] 
    protected float rayOffset;

    protected int attackCombo = 0;//攻撃回数

    [SerializeField, Header("スタートポジション")]
    protected Vector3 startPos;
    [SerializeField, Header("スタート時の向き")]
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

    protected PlayerInput plaIn;//インプットシステム

    protected CharacterController charaCon;//キャラクターコントローラー

    protected Animator animCon;//アニメーター

    [SerializeField,Header("ゲーム開始")]
    protected bool gameStart = false;//プレイヤーを操作できるようにする。

    [SerializeField, Header("ゲームオーバー")]
    protected bool gameOver = false;//ゲームオーバー判定

    protected bool starting = true;//開幕ジャンプ中アニメ封じ用

    private bool isChanegeAction = false;//行動中判定

    protected bool isJumpping = false;//ジャンプ中判定

    private bool onJumpping = false;//二段ジャンプ中判定

    protected bool falling = false;//空中判定

    protected bool landing = true;//着地判定

    protected bool attacking = false;//攻撃中判定

    protected bool jumpAttackingFall = false;//空中攻撃中判定

    protected bool avoidancing = false;//回避中判定

    protected bool hitting = false;//ヒット中判定

    protected bool isDown = false;//ダウン中判定

    [SerializeField, Header("地面判定リスト")]
    protected List<bool> groundPlayerList = new List<bool>();

    [SerializeField, Header("地面レイヤーリスト")]
    protected List<LayerMask> groundLayerList = new List<LayerMask>();

    private float cool = 0.3f;//コルーチンの間隔


    //---プロパティ---//
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

        Debug.Log("や");

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

    protected void Fall()//落下判定取得時
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
    /// プレイヤーの回転処理を行うメソッドです。
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
    /// プレイヤーにかかる重力負荷を演算するメソッドです。
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
    /// groundPlayerListのカウント数を上限とし、地面レイヤーを繰り返し判定するメソッドです。
    /// </summary>
    protected void CheckGroundPlayerList()
    {
        for (int i = 0; i < groundPlayerList.Count; i++)
        {
            groundPlayerList[i] = CheckGroundLayer(groundLayerList[i]);
        }
    }
    /// <summary>
    /// rayを基にプレイヤーが地面に着いているか、いないかを判定するメソッドです。
    /// </summary>
    /// <param name="groundLayer"></param>
    /// <returns></returns>
    protected bool CheckGroundLayer(LayerMask groundLayer)
    {
        // 放つ光線の初期位置と姿勢
        // 若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
        var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

        // Raycastがhitするかどうかで判定
        // レイヤの指定を忘れずに
        return Physics.Raycast(ray, rayLength, groundLayer);
    }

    /// <summary>
    /// プレイヤーが地面の上にいるか、もしくは、いないかの判定の結果を返します。引数をtrueにすると「地面の上にいるか」、falseにすると「地面の上にいないか」をそれぞれ判定するようになります。
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

    protected void Grounded()//着地時に使用
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
        soundManager.OneShot_Player_Sound(4);//ジャンプ着地サウンド
    }

    private void OnDrawGizmos()
    {
        // 接地判定時は緑、空中にいるときは赤にする
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

            soundManager.OneShot_Player_Sound(2);//ジャンプサウンド

            isJumpping = true;
        }
        else if (isJumpping && onJumpping)
        {
            Debug.Log("s2");

            animCon.SetBool("JumpAnim", false);

            animCon.SetBool("OnJump", true);

            playerVelocity += jumpForse % 80;

            soundManager.OneShot_Player_Sound(3);//二段ジャンプサウンド

            onJumpping = false;
        }

       


    }



    /// <summary>
    /// プレイヤーの足音を出すメソッド。アニメーションイベントで使用
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

    //----------------------ジャンプアニメーションイベントで制御----------------------//
    public void OnJumpTrue()//アニメーションイベントで制御
    {
        onJumpping = true;
    }

    public void isJumpFalse()//アニメーションイベントで制御
    {
        isChanegeAction = false;
        animCon.SetBool("JumpAnim", false);
        //isJumpping = true; 
    }

    public void OnJumpFalse()//アニメーションイベントで制御
    {
        isChanegeAction = false;
        animCon.SetBool("OnJump", false);
    }

    //--------------------------------------------------------------------------------//

    //----------------------着地アニメーションイベントで制御----------------------//
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
        {//playerVelocityの初期化

            playerVelocity = 0;
            Debug.Log("s0");
        }

        falling = false;

        animCon.SetBool("Landing", false);
    }

    //--------------------------------------------------------------------------------//
}
