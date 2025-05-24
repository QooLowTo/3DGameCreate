
using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
public  class BossGolem : Enemy
{
    //[SerializeField,Header("ボスの体力")] 
    //private int bossHp = 500;
    [SerializeField,Header("ボスの現在の体力")]
    private int liveBossHP;

    //[SerializeField,Header("ボスの防御力")]
    //private int bossDefence;

    //[SerializeField,Header("ボス撃破時の獲得経験値量")]
    //private int getExp;

    private float bossSpeed;

    private float attackCool = 0.8f;

    [SerializeField,Header("AttackCoolに返す値")]
    float returnAtttackcool = 4.0f;

    private float bossAnimSpeed;

    [SerializeField,Header("ターゲットとの距離判定の値")]
    private float forwardDistance;

    [SerializeField,Header("ボスのジャンプ力")]
    private float jumpForce = 100f;

    //[SerializeField,Header("ボスの突進力")]
    //private float forwardForce = 100f;


    private Vector3 directionToPlayer;//急接近、急後退用のベクトル取得

    private Vector3 fowardDirection;//directionToPlayerのAddForce用に使うベクトル(急接近)

    private Vector3 BackDirection;//directionToPlayerのAddForce用に使うベクトル(急後退)


    private int attakCount = 0;

 

    [SerializeField,Header("クールダウン時間の値")]
    private float coolDownTime = 7f;

    private float setCoolDownTime;

    //private int hitDamage;

    //private float damageUIPos = 2f;

    private Collider thisBossCollider;

    private BoxCollider hornAttackCollider;
 
    private BoxCollider attackCollider;

    private SphereCollider impactCollider;

    [SerializeField]
    private GameObject findHornAttackColliderObj;

    [SerializeField]
    private GameObject findAttackColliderObj;

    [SerializeField]
    private GameObject findImpactColliderObj;

    //private GameObject findGM;

    //private GameObject findBattleManager;

    //private GameObject target;

    [SerializeField]
    private GameObject escapeObject;

    [SerializeField]
    private GameObject findBossHPSlide;

    [SerializeField] 
    private float rayLength = 1f;

    [SerializeField]
    private float rayOffset;

    [SerializeField]
    LayerMask groundLayers = default;

    private bool hitting = false;//ダメージを受けるとON。多段ヒット防止用

    private bool bossAttacking = false;//ボスの攻撃中の判定用

    private bool bossGrounded = false;   //地面にいるかどうか判定用

    private bool bossApproached = false;//プレイヤーに近づくとON

    private bool bossActionStart = true;//スタート時のボスの動きの制御用

    private bool bossDie = false;//ボスの死亡判定

    //private Player_Status_Controller plasta;

    private BossHealthBar bossHPSlide;

    private GameManager gameManager;

    //private BattleManager battleManager;

    //private NavMeshAgent myAgent;

    //private Animator animCon;//アニメーター

    //private Rigidbody rb;

    [SerializeField]
    private List<GameObject> bossEffects = new List<GameObject>();

    public float AttackCool { get => attackCool; set => attackCool = value; }
    public bool BossActionStart { get => bossActionStart; set => bossActionStart = value; }
    public bool BossAttacking { get => bossAttacking; set => bossAttacking = value; }
    public GameObject FindBossHPSlide { get => findBossHPSlide; set => findBossHPSlide = value; }
    public BoxCollider HornAttackCollider { get => hornAttackCollider; set => hornAttackCollider = value; }
    public BoxCollider AttackCollider { get => attackCollider; set => attackCollider = value; }
    public SphereCollider ImpactCollider { get => impactCollider; set => impactCollider = value; }
    

    private enum ActionType
    { 
      TowardOn,  
      HornAttack,
      CoolDown,
      Wait,
      Attack,
      Jump,
      Lading

    }ActionType actionType = ActionType.TowardOn;

   

    // Start is called before the first frame update
    void Awake()
    {
        StartEnemySetting();

        //target = GameObject.FindWithTag("Player");

        //findGM = GameObject.FindWithTag("GameManager");

        //findBattleManager = GameObject.FindWithTag("BattleManager");

        //gameManager = findGM.GetComponent<GameManager>();

        //battleManager = findBattleManager.GetComponent<BattleManager>();

        //plasta = target.GetComponent<Player_Status_Controller>();

        //animCon = GetComponent<Animator>();

        //myAgent = GetComponent<NavMeshAgent>();

        //rb = GetComponent<Rigidbody>();

        bossHPSlide = findBossHPSlide.GetComponent<BossHealthBar>();

        //forwardForce = forwardForce + (forwardForce * Time.deltaTime);

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
        //HornAttackCollider = GetComponent<BoxCollider>();
        //attackcollider = GetComponent<BoxCollider>();

        IEnumerator BossActionStart()
        { 
            yield return new WaitForSeconds(6);

            bossActionStart = false;

            findBossHPSlide.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossDie||bossActionStart) return;

        myAgent.SetDestination(target.transform.position);

        var targetPos = new Vector3(target.transform.position.x,0f,target.transform.position.z);//ターゲットのポジション

        var BossPos = new Vector3(transform.position.x,0f,transform.position.z);

        var distanceToTarget = Vector3.Distance(this.transform.position, targetPos);//ターゲットとボスとの距離

        myAgent.speed = bossSpeed;

        //directionToPlayer = (targetPos - BossPos).normalized;

        //if (actionType != ActionType.CoolDown && thisBossCollider.enabled){
            
        //    thisBossCollider.enabled = false;

        //}

        if (distanceToTarget < forwardDistance){
              
            bossApproached = true;

        }else{

            bossApproached = false;

        }

            if (!bossApproached && bossGrounded){

            attackCool = returnAtttackcool;

            attakCount = 0;

            coolDownTime = setCoolDownTime;

            actionType = ActionType.TowardOn;

            }
        
        switch (actionType)
        { 
            case ActionType.TowardOn:

                if (actionType == ActionType.CoolDown || actionType == ActionType.Jump || actionType == ActionType.Lading) return;

                bossSpeed += Time.deltaTime;

                //if (rb.isKinematic != false || myAgent.enabled != false)
                //{
                //    rb.isKinematic = true;
                //    myAgent.enabled = true;
                //}
               
              

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

                if (attakCount < 3)
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

                    //rb.AddForce(0, jumpForce, 0f, ForceMode.Impulse);

                    animCon.SetTrigger("Jump");
 
                }
                 

            break;

                case ActionType.Lading:

                thisBossCollider.enabled = false;

                animCon.ResetTrigger("Jump");

                if (bossGrounded)
                {
                   
                    animCon.SetTrigger("Land");

                    attakCount = 0;

                }

                break;
        }

        if (bossHPSlide != null){

            bossHPSlide.SetHealth(liveBossHP);

        }

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

        if (bossAnimSpeed <= 3f) { 

        bossAnimSpeed = bossSpeed/2.0f;

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

            //Invoke("HittingFalse", 0.2f);

            
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

            case ActionType.Lading:

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
        soundManager.OneShot_Boss_Action_Sound(2);
    }

    public void AttackOff()//アニメーションイベントで制御
    {
        attackCollider.enabled = false;
        attakCount++;
        attackCool = returnAtttackcool;
        actionType = ActionType.Wait;
    }

    public void JumpOn()//アニメーションイベントで制御
    {
        rb.AddForce(0, jumpForce, 0f, ForceMode.Impulse);
        rayLength = 0;
        actionType = ActionType.Lading;
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
        soundManager.OneShot_Boss_Action_Sound(3);
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

        //plasta.PlayerExp += getExp;

        OffCollider();

        animCon.SetBool("Die", true);

        soundManager.OneShot_Boss_Action_Sound(4);

        StartCoroutine(BossDieEffectAndDestroy());

       

      

        IEnumerator BossDieEffectAndDestroy()
        {
            battleManager.Boss_Die_Vibration();

            Time.timeScale /= 2.0f;

            bossDie = true;

            yield return new WaitForSeconds(5f);

            Time.timeScale = 1;

            Instantiate(bossEffects[3], gameObject.transform.position, Quaternion.identity);

            Destroy(findBossHPSlide);

            escapeObject.SetActive(true);

            soundManager.OneShot_Boss_Action_Sound(5);

            battleManager.BossBattle = false;

            Destroy(gameObject, 0.15f);
        }
    }

   


  
    private void OnDrawGizmos()
    {
        // 接地判定時は緑、空中にいるときは赤にする
        Gizmos.color = bossGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayLength);
    }

    //public void ActionStart()
    //{ 
    //    bossActionStart = false;

    //    findBossHPSlide.SetActive(true);
    //}
    //private void MoveLimit(float _yLimit, float yLimit)
    //{
    //    Vector3 LimitPos = transform.position;

    //    float _xLimit = -21f;  float xLimit = 30f;

    //    //_yLimit = 0f;    _yLimit = 5f;

    //    float _zLimit = -30f;  float zLimit = 30f;

    //    LimitPos.x = Mathf.Clamp(LimitPos.x, _xLimit, xLimit);
    //    LimitPos.y = Mathf.Clamp(LimitPos.y, _yLimit, yLimit);
    //    LimitPos.z = Mathf.Clamp(LimitPos.z, _zLimit, zLimit);

    //    transform.position = LimitPos;
    //}


   
}
