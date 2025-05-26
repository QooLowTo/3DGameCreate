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
public class Player_Battle_Controller : Player
{
    protected float returnSpeed;//ダッシュ解除時にplayerSoeedに返す値
    
    [SerializeField,Header("ダッシュの初速度")]
    protected int doDashFastSpeed = 0;//ダッシュの初速度

    [SerializeField, Header("DoTweenの移動量")]
    protected float doAvoidancePower = 20f;//DoTweenの移動量

    [SerializeField,Header("DoTweenの移動時間")]
    protected float doTime = 0.5f;//DoTweenの移動時間

    [SerializeField,Header("プレイヤーの武器")]
    protected GameObject myWepon;//プレイヤーの武器

    [SerializeField,Header("トレイル")]
    protected GameObject weponEffect;//トレイル

    [SerializeField, Header("ジャンプ攻撃時の判定")]
    private Collider jumpAttackCol;//ジャンプ攻撃時の判定

    protected RigBuilder rig;//リグビルダー(ダッシュアニメーション用)
    protected InputAction dashInput;

    protected BattleManager battleManager;
    [SerializeField]
    protected GameObject findBattleManager;

    protected bool isChanegeAction = false;//行動中判定

    protected bool isDash = false;//ダッシュ中判定

    protected bool onJumpping = false;//二段ジャンプ中判定

    protected bool approached = false;//敵との距離

    protected bool stopCombo = false;//コンボ封じ用

    protected bool attackingLook = false;//ロックオン中判定

    protected bool forwardAvoidancing = false;//ロックオンオン中の前方向の回避の制御用。

   

 
   
    public RigBuilder Rig { get => rig; set => rig = value; }
  
    public bool IsDown { get => isDown; set => isDown = value; }
    public bool Avoidancing { get => avoidancing; set => avoidancing = value; }
    public bool Hitting { get => hitting; set => hitting = value; }
    public bool Attacking { get => attacking; set => attacking = value; }
    public bool AttackingLook { get => attackingLook; set => attackingLook = value; }
    public bool Approached { get => approached; set => approached = value; } 
    public bool ForwardAvoidancing { get => forwardAvoidancing; set => forwardAvoidancing = value; }
    public bool StopCombo { get => stopCombo; set => stopCombo = value; }
    //public int AttackCombo { get => attackCombo; set => attackCombo = value; }
    public float MoveInputAbs { get => moveInputAbs; set => moveInputAbs = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public GameObject MyWepon { get => myWepon; set => myWepon = value; }
  
    // Start is called before the first frame update

    private void Start()
    {


        StartCoroutine(Starting());

        plasta = GetComponent<Player_Status_Controller>();

        plaIn = GetComponent<PlayerInput>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        battleManager = findBattleManager.GetComponent<BattleManager>();

        charaCon = GetComponent<CharacterController>();

        animCon = GetComponent<Animator>();

        rig = GetComponent<RigBuilder>();

        StartPlayerSet();

        //plaIn.SwitchCurrentActionMap("Player");

        dashInput = plaIn.actions["Dash"];

       

        returnSpeed = playerSpeed;

    }
    

   
    private void FixedUpdate()
    {
        if (gameOver ||/* !battleManager.GameStart*/ !gameStart || !charaCon) return;

        CheckGroundPlayerList();

        if (!attacking&& !jumpAttackingFall && !isDown && !avoidancing && !isChanegeAction&& !hitting)
        {
            //animCon.SetFloat("MoveInput", moveInputAbs);

            PlayerMoveInputControl();
        }
        else
        {
            animCon.SetFloat("MoveInput", 0);
        }  

        GravityLoad();

        if (avoidancing)
        {
            if (myWepon.GetComponent<BoxCollider>().enabled)
            {
                OffCollider();
            }

            if (weponEffect.GetComponent<TrailRenderer>().emitting)
            {
                OffEffect();
            }
        }

        if (isDown||hitting)
        {
            AllCancel();
        }

        if (!isChanegeAction && !attacking && !isDown && !hitting)
        {

            PlayerMoveControl();

            if (moveInputAbs != 0 && !isJumpping && groundPlayerList[1]/*階段だったら*/)
            {

                MovingLimit();

            }
        }


        PlayerGravityControl();


        var ispressed = dashInput.IsPressed();

        if (ispressed && moveInputAbs != 0)
        {

            OnDash();

        }
        else
        {

            OffDash();

        }

        if ((moveInputAbs != 0) && !isChanegeAction && !attacking && !isJumpping && !avoidancing && !isDown && OnGroundLayer(true))
        {
            rig.enabled = true;
        }
        else
        {
            rig.enabled = false;
        }


        //if (isJumpping&&onJumpping){//二段ジャンプ処理

        //    DoubleJump();

        //}



        if (landing && OnGroundLayer(false))
        {

            if (starting || hitting) return;


            Fall();
        }


        if (!landing && OnGroundLayer(true))
        {

            landing = true;

            if (jumpAttackingFall)
            {

                AttackGrounded();

            }
            else
            {

                Grounded();
            }
        }

        if (!attacking && stopCombo)
        {

            stopCombo = false;

        }

        if (plasta.LivePlayerHP <= 0)
        {

            Die();
        }
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (battleManager.GameOver || !battleManager.GameStart || !charaCon.enabled) return;



    //    //if (!isChanegeAction && !attacking && !isDown && !hitting)
    //    //{

    //    //    PlayerMoveControl();

    //    //    if (moveInputAbs != 0 && !isJumpping && groundPlayerList[1]/*階段だったら*/)
    //    //    {

    //    //        MovingLimit();

    //    //    }
    //    //}


    //    //PlayerGravityControl();


    //    //var ispressed = dashInput.IsPressed();

    //    //if (ispressed && moveInputAbs != 0)
    //    //{

    //    //    OnDash();

    //    //}
    //    //else
    //    //{

    //    //    OffDash();

    //    //}

    //    //if ((moveInputAbs != 0) && !isChanegeAction && !attacking && !isJumpping && !avoidancing && !isDown && OnGroundLayer(true))
    //    //{
    //    //    rig.enabled = true;
    //    //}
    //    //else
    //    //{
    //    //    rig.enabled = false;
    //    //}


    //    ////if (isJumpping&&onJumpping){//二段ジャンプ処理

    //    ////    DoubleJump();

    //    ////}



    //    //if (landing && OnGroundLayer(false))
    //    //{

    //    //    if (starting || hitting) return;


    //    //    Fall();
    //    //}


    //    //if (!landing && OnGroundLayer(true))
    //    //{

    //    //    landing = true;

    //    //    if (jumpAttackingFall)
    //    //    {

    //    //        AttackGrounded();

    //    //    }
    //    //    else
    //    //    {

    //    //        Grounded();
    //    //    }
    //    //}

    //    //if (!attacking && stopCombo)
    //    //{

    //    //    stopCombo = false;

    //    //}

    //    //if (plasta.LivePlayerHP <= 0)
    //    //{

    //    //    Die();
    //    //}


    //}
        
       
        //void DoubleJump()//二段ジャンプ処理
        //{
        //    if (plaIn.actions["Jump"].triggered && OnGroundLayer(false)){
        //        animCon.SetBool("JumpAnim", false);


        //        animCon.SetBool("OnJump", true);

        //        playerVelocity += jumpForse % 80;

        //        gameManager.OneShotPlayerSound(3);

        //        onJumpping = false;

        //    }
        //}
        /// <summary>
        /// プレイヤーがダッシュアクションを行った際に呼び出されるメソッド。
        /// </summary>
        protected void OnDash()
        {
            if (isDash || attacking || isDown || avoidancing || hitting || OnGroundLayer(false)) return;

            animCon.SetBool("Dash", true);

            soundManager.OneShot_Player_Sound(1);//ダッシュサウンド

            DoForward(doDashFastSpeed);//ダッシュ時の瞬間加速

            playerSpeed += playerSpeed % 50f;

            isDash = true;

        }
    /// <summary>
    /// プレイヤーがダッシュを辞めた際に呼び出されるメソッド。
    /// </summary>
        protected void OffDash()
        {
            if (!isDash) return;

            animCon.SetBool("Dash", false);

            playerSpeed = returnSpeed;

            isDash = false;

      
        }

    protected void MovingLimit()
        {
               var moveLimit = Vector3.down;

                var LimitDelta = moveLimit + (moveLimit * Time.deltaTime);

                charaCon.Move(LimitDelta * playerSpeed);
          
        }




   　　 protected void Die()
        {

            AllCancel();

            rig.enabled = false;

            animCon.SetLayerWeight(1, 1f);

            animCon.SetTrigger("Die");

            charaCon.enabled = false;
         
            soundManager.BGMStop();

            soundManager.OneShot_Player_Sound(11);//ゲームオーバーサウンド

            battleManager.OnGameOver();//ゲームオーバーメニュー表示

            gameOver = true;//ゲームオーバー判定
        }

   

    public void FallingDie()
    {
        rig.enabled = false;

        soundManager.OneShot_Player_Sound(12);//落下死サウンド

        soundManager.BGMStop();

        battleManager.OnGameOver();

        gameOver = true;

    }

    

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed || hitting || isDown || avoidancing) return;

        if (OnGroundLayer(false)&&!jumpAttackingFall)
        {
           

            jumpActioningForce = 10;

            jumpAttackingFall = true;

            attackingLook = true;

            OnCollider();

            animCon.SetBool("JumpAnim", false);//ジャンプアニメーション停止

            animCon.SetBool("OnJump", false);//ジャンプアニメーション停止

            animCon.SetBool("Falling", false);

            animCon.SetBool("Landing", false);//着地アニメーション防止

            animCon.SetBool("JumpAttackFall",true);
        }

        if (jumpAttackingFall) return;

        if (!isChanegeAction)
        {

            attacking = true;

            attackingLook = true;

            animCon.SetBool("Attacking", true);//AnySatateの制御用

            attackCombo++;
            animCon.SetInteger("AttackCombo", attackCombo);
            stopCombo = true;

            if (attackCombo <= 1)
            {
                animCon.SetTrigger("Attack");
            }

        }
        else
        {
            animCon.SetBool("AvoAttack",true);
            attackingLook = true;

        }

    }

    public void Avoidance(InputAction.CallbackContext context)
    {
        if (!context.performed || avoidancing || hitting || isDown || isJumpping || onJumpping || OnGroundLayer(false)) return;

         if(moveInputAbs == 0 ){
            animCon.SetBool("BackAvoidance", true);
         }else if (attacking && moveInput.x >= 0.1f){
            animCon.SetBool("RightAvoidance", true);
         }else if (attacking && moveInput.x <= -0.1f ){
            animCon.SetBool("LeftAvoidance", true);
         }else if (moveInputAbs >= 0.1f){
            animCon.SetBool("ForwardAvoidance", true);

            forwardAvoidancing = true;  
         } 

        OffAttacking();

        soundManager.OneShot_Player_Sound(5);//回避サウンド

        isChanegeAction = true;
        avoidancing = true;
    }

 

    //----------------------アタックアニメーションイベントで制御----------------------//

    public void AE_OnCollider()
    {
        OnCollider();
    }

    public void AE_OffCollider()
    { 
        OffCollider();
    }

    public void AE_OffComboStop()
    {
        OffComboStop();
    }

    public void AE_NextAttack()
    {
        NextAttack();
    }

    public void AE_OffAttackingLook()
    {
       OffAttackingLook();
    }

    public void AE_OffAttacking()
    {
        OffAttacking();
    }

    public void AE_OnJumpAttackCollider()
    {
        OnJumpAttackCollider();
    }

    public void AE_OffJumpAttackCollider()
    {
        OffJumpAttackCollider();
    }

    public void AE_AttackForward(int DofowardPower)
    {
        DoForward(DofowardPower);
    }

    public void AE_OffEffect()
    {
        OffEffect();
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

    private void OnJumpAttackCollider()
    { 
    jumpAttackCol.GetComponent<SphereCollider>().enabled = true;
        soundManager.OneShot_Player_Sound(14);//ジャンプ攻撃着地サウンド
    }

    private void OffJumpAttackCollider()
    {
        jumpAttackCol.GetComponent<SphereCollider>().enabled = false;
        animCon.SetBool("JumpAttack", false);
        falling = false;

    }

    private void AttackForward(int DofowardPower)
    {
        DoForward(DofowardPower);
    }

    protected void OffEffect()
    { 
      weponEffect.GetComponent<TrailRenderer>().emitting = false;
    }

   

    protected void DoForward(int DofowardPower)
    {
        if (approached) return;

        DOTween.To(() => transform.position,
                    v =>
                    {
                        Vector3 velocity = (v - transform.position) * Time.deltaTime;
                        CharaCon.Move(velocity);
                    },
             transform.position + (transform.forward * DofowardPower), doTime);
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
        if (forwardAvoidancing)
        { 
        forwardAvoidancing = false;
        }
    }

    public void CancelAvoidanceAction()
    { 
        isChanegeAction = false;

        attackingLook = false;

        animCon.SetBool("AvoAttack", false);

    }


    private void OnCollider()
    {
        myWepon.GetComponent<BoxCollider>().enabled = true;
        weponEffect.GetComponent<TrailRenderer>().emitting = true;
        soundManager.OneShot_Player_Sound(0);//剣の素振りサウンド
    }
    protected void OffCollider()
    {
        myWepon.GetComponent<BoxCollider>().enabled = false;
    }

    private void OffComboStop()
    {
        stopCombo = false;

    }

    private void NextAttack()
    {
        animCon.SetTrigger("NextAttack");
    }

    private void OffAttackingLook()
    {
        attackingLook = false;
    }


    public void OnHit(int hitDamage)
    { 
        hitting = true;

        plasta.LivePlayerHP -= hitDamage;

        battleManager.DamageText(charaCon,hitDamage,0.2f);

        plasta.SetLiveHP();

        soundManager.OneShot_Player_Sound(6);//被ダメージサウンド

        animCon.SetTrigger("Hit");

    }
    public void OffHit()//アニメーションイベントで制御
    { 
        hitting = false;

        if (OnGroundLayer(true)){

            Landing();
        }
    }

    public void OnKnockBack(int hitDamage,float knockBackPower,float knockBackSpeed,Transform enemyPos)
    {
        plasta.LivePlayerHP -= hitDamage;

        battleManager.DamageText(charaCon, hitDamage, 0.2f);

        plasta.SetLiveHP();

        soundManager.OneShot_Player_Sound(7);//被ノックバックサウンド

        animCon.SetTrigger("knockback");

        animCon.SetBool("knockbacking", true);

        DOTween.To(() => transform.position,
                  v =>
                  {
                      Vector3 velocity = (v - transform.position) * Time.deltaTime;
                      CharaCon.Move(velocity);
                  },
                  transform.position + (enemyPos.transform.forward * knockBackPower), knockBackSpeed);
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
        animCon.SetBool("JumpAttackFall", false);
        animCon.SetBool("JumpAttack", false);

        isDash = false;
        isJumpping = false;
        onJumpping = false;
        jumpAttackingFall = false;

        CancelAvoidance("BackAvoidance");
        CancelAvoidance("RightAvoidance");
        CancelAvoidance("LeftAvoidance");
        CancelAvoidance("ForwardAvoidance");
    }

}
