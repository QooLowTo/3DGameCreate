
using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

/// <summary>
/// ボスのゴーレムの動きを制御するクラスです。
/// </summary>
public  class BossGolem : Enemy
{

    [SerializeField,Header("ボスの現在の体力")]
    private int liveBossHP;

    private float bossSpeed; //説明書いて

    private float attackCool = 0.8f;　//説明書いて

    [SerializeField,Header("AttackCoolに返す値")]
    float returnAtttackcool = 4.0f;

    private float bossAnimSpeed;　//説明書いて
　
    [SerializeField,Header("ターゲットとの距離判定の値")]
    private float forwardDistance;

    [SerializeField,Header("ボスのジャンプ力")]
    private float jumpForce = 100f;


    private Vector3 directionToPlayer;//急接近、急後退用のベクトル取得

    private Vector3 fowardDirection;//directionToPlayerのAddForce用に使うベクトル(急接近)

    private Vector3 backDirection;//directionToPlayerのAddForce用に使うベクトル(急後退)


    private int attackCount = 0;　//説明書いて

 

    [SerializeField,Header("クールダウン時間の値")]
    private float coolDownTime = 7f;

    private float setCoolDownTime;　//説明書いて

    private Collider thisBossCollider;　//説明書いて

    private BoxCollider hornAttackCollider;　//説明書いて
 
    private BoxCollider attackCollider;　//説明書いて

    private SphereCollider impactCollider;　//説明書いて

    [SerializeField]
    private GameObject findHornAttackColliderObj;　//説明書いて

    [SerializeField]
    private GameObject findAttackColliderObj;　//説明書いて

    [SerializeField]
    private GameObject findImpactColliderObj;　//説明書いて

    [SerializeField]
    private GameObject escapeObject;　//説明書いて

    [SerializeField]
    private GameObject findBossHPSlide;　//説明書いて

    [SerializeField] 
    private float rayLength = 1f;　//説明書いて

    [SerializeField]
    private float rayOffset;　//説明書いて

    [SerializeField]
    LayerMask groundLayers = default;　//説明書いて

    private bool hitting = false;//ダメージを受けるとON。多段ヒット防止用

    private bool bossAttacking = false;//ボスの攻撃中の判定用

    private bool bossGrounded = false;   //地面にいるかどうか判定用

    private bool bossApproached = false;//プレイヤーに近づくとON

    private bool bossActionStart = true;//スタート時のボスの動きの制御用

    private bool bossDie = false;//ボスの死亡判定

    private BossHealthBar bossHPSlide;　//説明書いて

    private GameManager gameManager;　//説明書いて

    [SerializeField]
    private List<GameObject> bossEffects = new List<GameObject>();　//説明書いて

    //プロパティ
    public float AttackCool { get => attackCool; set => attackCool = value; }
    public bool BossActionStart { get => bossActionStart; set => bossActionStart = value; }
    public bool BossAttacking { get => bossAttacking; set => bossAttacking = value; }
    public GameObject FindBossHPSlide { get => findBossHPSlide; set => findBossHPSlide = value; }
    public BoxCollider HornAttackCollider { get => hornAttackCollider; set => hornAttackCollider = value; }
    public BoxCollider AttackCollider { get => attackCollider; set => attackCollider = value; }
    public SphereCollider ImpactCollider { get => impactCollider; set => impactCollider = value; }
    

    /// <summary>
    /// ボスの行動を制御するための列挙型です。
    /// </summary>
    private enum ActionType
    {
        TowardOn, //説明書いて。例：プレイヤーに向かう
        HornAttack, //説明書いて。
        CoolDown, //説明書いて。
        Wait, //説明書いて。
        Attack, //説明書いて。
        Jump, //説明書いて。
        Landing //説明書いて。

    }
    ActionType actionType = ActionType.TowardOn; //説明書いて

   
    void Awake()
    {
        //シンプルにこのブロック内にどういう処理をしているか説明書いてください。
        StartEnemySetting();

        bossHPSlide = findBossHPSlide.GetComponent<BossHealthBar>();

        jumpForce = jumpForce + (jumpForce * Time.deltaTime);

        thisBossCollider = GetComponent<Collider>();

        hornAttackCollider = findHornAttackColliderObj.GetComponent<BoxCollider>();

        attackCollider = findAttackColliderObj.GetComponent<BoxCollider>();

        impactCollider = findImpactColliderObj.GetComponent<SphereCollider>();


        battleManager.BossBattle = true;

        liveBossHP = enemyHp;

        setCoolDownTime = coolDownTime;

        bossHPSlide.SetMaxHealth(enemyHp);

        thisBossCollider.enabled = false;

        StartCoroutine(BossActionStart());

        IEnumerator BossActionStart()
        { 
            //マジックナンバー発見！変数化するか、なぜ６秒なのか説明書いてください。
            yield return new WaitForSeconds(6);

            bossActionStart = false;

            findBossHPSlide.SetActive(true);
        }
    }

    void Update()
    {
        if (bossDie||bossActionStart) return;

        myAgent.SetDestination(target.transform.position);

        var targetPos = new Vector3(target.transform.position.x,0f,target.transform.position.z);//ターゲットのポジション

        var bossPos = new Vector3(transform.position.x,0f,transform.position.z);

        var distanceToTarget = Vector3.Distance(this.transform.position, targetPos);//ターゲットとボスとの距離

        myAgent.speed = bossSpeed;


        //ブロックの説明書いて！
        if (distanceToTarget < forwardDistance)
        {

            bossApproached = true;

        }
        else
        {

            bossApproached = false;

        }

            if (!bossApproached && bossGrounded){

            attackCool = returnAtttackcool;

            attackCount = 0;

            coolDownTime = setCoolDownTime;

            actionType = ActionType.TowardOn;

            }
        
        switch (actionType)
        { 
            case ActionType.TowardOn:

                if (actionType == ActionType.CoolDown || actionType == ActionType.Jump || actionType == ActionType.Landing) return;

                bossSpeed += Time.deltaTime;
       

                if (bossApproached && bossSpeed > 1){

                    rb.isKinematic = false;

                    actionType = ActionType.HornAttack;

                }
              
                
                break;
            
         

            case ActionType.HornAttack:

                bossSpeed = 0;

                bossAnimSpeed = 0;

                animCon.SetTrigger("Hit");

            break;

            case ActionType.CoolDown:

                rb.isKinematic = true;

                thisBossCollider.enabled = true;

                animCon.SetTrigger("IdleAction");

                coolDownTime -= Time.deltaTime;

                if (bossApproached && coolDownTime < 0)
                {
                    actionType = ActionType.Attack;

                    coolDownTime = setCoolDownTime;

                }
             

            break;

            case ActionType.Wait:

                if (!thisBossCollider.enabled)
                {

                    thisBossCollider.enabled = true;

                }

                //３はマジックナンバーです。変数化するか、なぜ３なのか説明書いてください。
                if (attackCount < 3)
                {

                    attackCool -= Time.deltaTime;

                    LookPlayer();

                    if (attackCool < 0)
                    {

                        actionType = ActionType.Attack;

                    }
                }
                else
                {

                    actionType = ActionType.Jump;
                }

                break;

            case ActionType.Attack:

                thisBossCollider.enabled = false;

                animCon.SetTrigger("Hit2");

                break;

            case ActionType.Jump:

                if (bossGrounded)
                { 
                    rb.isKinematic = false;

                    myAgent.enabled = false;        

                    animCon.SetTrigger("Jump");
 
                }
                 

            break;

                case ActionType.Landing:

                thisBossCollider.enabled = false;

                animCon.ResetTrigger("Jump");

                if (bossGrounded)
                {
                   
                    animCon.SetTrigger("Land");

                    attackCount = 0;

                }

                break;
        }

        //ブロック説明書いて
        if (bossHPSlide != null)
        {

            bossHPSlide.SetHealth(liveBossHP);

        }
          //ブロック説明書いて
        if (liveBossHP < 0){

            bossHPSlide.HealthDeath();

            BossOnDie();

            liveBossHP = 0;

        }

        void LookPlayer()
        {
            var targetPos = new Vector3(target.transform.position.x, 0f, target.transform.position.z);

            transform.DOLookAt(targetPos, 1f);
        }

    }

    private void FixedUpdate()
    { 
        bossGrounded = CheckGrounded();

        //3fはマジックナンバーです。変数化するか、なぜ3fなのか説明書いてください。
        if (bossAnimSpeed <= 3f)
        {

            //2と割る理由
            bossAnimSpeed = bossSpeed / 2.0f;

        } 

        animCon.SetFloat("Walk",bossAnimSpeed);

        if (actionType == ActionType.HornAttack){

            fowardDirection = transform.forward * forwardForce;

            rb.AddForce(fowardDirection, ForceMode.Impulse);

        }

        bool CheckGrounded()
        {
            // 放つ光線の初期位置と姿勢
            // 若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
            var ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: Vector3.down);

            // Raycastがhitするかどうかで判定
            // レイヤの指定を忘れずに
            return Physics.Raycast(ray, rayLength, groundLayers);
        }
    }
    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "PlayerWeapon" && hitting == false)
        {
            OnBossHit();
        }

        void OnBossHit()
        {
            enemyDamage = battleManager.DamegeCalculation(enemyDefence, plaSta.PlayerAttackPower);//ダメージの計算結果をダメージ変数に代入。

            liveBossHP -= enemyDamage;//HP - ダメージ

            battleManager.DamageText(thisBossCollider, enemyDamage, damageUIPos);//ダメージの表示。

            battleManager.Enemy_Hit_Effect(gameObject);

            soundManager.OneShot_Player_Sound(8);

            if (liveBossHP > 1){

                battleManager.Enemy_Hit_Vibration();

            }

            hitting = true;

            StartCoroutine(HittingFalse());
            
        }

        IEnumerator HittingFalse()
        {
            yield return new WaitForSeconds(0.2f);

            hitting = false;
        }
    }
    /// <summary>
    /// ボスがどの攻撃をしているかを判定するメソッド。
    /// </summary>
    /// <returns></returns>
    public string BossAttackActionType()
    {
        string AttackActionType = "";

        switch (actionType) { 
        
            case ActionType.Attack:

                AttackActionType = "Attack";

            break;

            case ActionType.HornAttack:

                AttackActionType = "HornAttack";

            break;

            case ActionType.Landing:

                AttackActionType = "Impact";

            break;

        }

       return AttackActionType;
    }
  

    public void HornAttackOn()//アニメーションイベントで制御
    {
        hornAttackCollider.enabled = true;
        soundManager.OneShot_Boss_Action_Sound(1);
    }
    public void HornAttackOff()//アニメーションイベントで制御
    {
        hornAttackCollider.enabled = false;
        animCon.ResetTrigger("Hit");
        actionType = ActionType.CoolDown;
    }
    public void AttackOn()//アニメーションイベントで制御
    { 
        attackCollider.enabled = true;
        //2の説明書いて
        soundManager.OneShot_Boss_Action_Sound(2);
    }

    public void AttackOff()//アニメーションイベントで制御
    {
        attackCollider.enabled = false;
        attackCount++;
        attackCool = returnAtttackcool;
        actionType = ActionType.Wait;
    }

    public void JumpOn()//アニメーションイベントで制御
    {
        rb.AddForce(0, jumpForce, 0f, ForceMode.Impulse);
        rayLength = 0;
        actionType = ActionType.Landing;
    }

    public void Fall()//アニメーションイベントで制御
    {
        rb.AddForce(0, -jumpForce * 1.5f, 0f, ForceMode.Impulse);
        rayLength = 2;
    }

    public void LandedImpact()//アニメーションイベントで制御
    {
        impactCollider.enabled = true;
    }
    public void Landed()//アニメーションイベントで制御
    {
        impactCollider.enabled = false;
        rb.isKinematic = true;
        myAgent.enabled = true;
        //３の説明書いて
        soundManager.OneShot_Boss_Action_Sound(3);

        //以下の90fはマジックナンバーです。変数化するか、なぜ90fなのか説明書いてください。
        //２の意味は？
        Instantiate(bossEffects[2], gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));

        StartCoroutine(ChangeAction());
        //Invoke("ChangeAction",1f);
        IEnumerator ChangeAction()
        {
            yield return new WaitForSeconds(1);

            if (bossApproached == true)
            {
                actionType = ActionType.Attack;

            }
            else
            {
                actionType = ActionType.TowardOn;
            }
        }
    }

   
   
   

   

    private void OffCollider()
    {
        thisBossCollider.enabled = false;
        hornAttackCollider.enabled = false;
        attackCollider.enabled = false;
    }

    private void BossOnDie()
    {
        soundManager.BGMStop();

        battleManager.GetExp(thisBossCollider, getExp);

        bossHPSlide.HealthDeath();

        OffCollider();

        animCon.SetBool("Die", true);

        //４の意味は？
        soundManager.OneShot_Boss_Action_Sound(4);

        StartCoroutine(BossDieEffectAndDestroy());

       

      

        IEnumerator BossDieEffectAndDestroy()
        {
            battleManager.Boss_Die_Vibration();

            //２で割る理由は？
            Time.timeScale /= 2.0f;

            bossDie = true;

            yield return new WaitForSeconds(5f);

            Time.timeScale = 1;
 
            //3は何？
            Instantiate(bossEffects[3], gameObject.transform.position, Quaternion.identity);

            Destroy(findBossHPSlide);

            escapeObject.SetActive(true);


            //５は何？
            soundManager.OneShot_Boss_Action_Sound(5);

            battleManager.BossBattle = false;
            //0.15f病後にオブジェクト削除
            Destroy(gameObject, 0.15f);
        }
    }

   


  
    private void OnDrawGizmos()
    {
        // 接地判定時は緑、空中にいるときは赤にする
        Gizmos.color = bossGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }
    
}
