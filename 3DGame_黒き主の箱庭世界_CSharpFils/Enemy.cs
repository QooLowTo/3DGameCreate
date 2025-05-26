using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniHumanoid;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// フィールドの敵を制御するクラスです。
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField,Header("敵のHP")]
    protected int enemyHp;

    [SerializeField, Header("敵の防御力")]
    protected int enemyDefence;

    [SerializeField] 
    private int enemyHitBarrierShield;

    [SerializeField, Header("敵の経験値")]
    protected int getExp;


    protected int enemyDamage;

    [SerializeField, Header("ダメージ表記のポジション")]
    protected float damageUIPos = 0.2f;

    [SerializeField]
    private float stoppingDistance;
  
    [SerializeField,Header("正面方向の力")] 
    protected float forwardForce;//敵が攻撃を受けた際のノックバック量

    private float defaltAgentSpeed;//NavAgentのスピードのデフォルトの値

    [SerializeField]
    private float attackCoolTime = 3;  
    
    [SerializeField]
    private float return_attackCoolTime = 5f;

    private float stopCoolTime = 0;

    [SerializeField] 
    private float setStopCoolTime;

    [SerializeField]
    private bool lookAtPlayer = false;

    private bool approached = false;//プレイヤーに近づくとON

    private bool hitting = false;//ダメージを受けるとON。多段ヒット防止用

    private bool stopping = false;//ダメージを受けるとON。ダメージ後のクールダウン用

    private bool enemyDie = false;//HPが0になるとON


    protected Player_Status_Controller plaSta;
  
    protected NavMeshAgent myAgent;

    protected Animator animCon;

    protected Rigidbody rb;

    [SerializeField] 
    private CapsuleCollider enemyThisCollider;//敵の物理判定のコライダー

    [SerializeField] 
    private BoxCollider enemyThisTriggerCollider;//敵の当たり判定のコライダー

    [SerializeField] 
    private Collider AttackCollider;//敵の攻撃判定のコライダー





    //private GameManager gameManager; 

    protected SoundManager soundManager;

    protected BattleManager battleManager;

    //private GameObject findGameManager;

    protected GameObject findSoundManager;

    protected GameObject findBattleManager;
      
    protected GameObject target; 
    // Start is called before the first frame update

    private enum ActionType
    { 
     Move,
     Stop,
     Attack
    }ActionType actionType;

    public bool EnemyDie { get => enemyDie; set => enemyDie = value; }
    public int EnemyHp { get => enemyHp; set => enemyHp = value; }
    public GameObject Target { get => target; set => target = value; }
    public Player_Status_Controller PlaSta { get => plaSta; set => plaSta = value; }
    public BattleManager BattleManager { get => battleManager; set => battleManager = value; }

    void Start()
    {  
        StartEnemySetting();

        //findGameManager = GameObject.FindWithTag("GameManager");
        //findBattleManager = GameObject.FindWithTag("BattleManager");
        //target = GameObject.FindWithTag("Player");

        //gameManager = findGameManager.GetComponent<GameManager>();
        //battleManager = findBattleManager.GetComponent<BattleManager>();
        //plasta = target.GetComponent<Player_Status_Controller>();
        //myAgent = GetComponent<NavMeshAgent>();
        //animCon = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
       
        //defaltAgentSpeed = myAgent.speed;
        //forwardForce = forwardForce + (forwardForce * Time.deltaTime);//Time.deltaTimeを掛けることで、フレームレートで影響を出さない。
    }

    protected void StartEnemySetting()
    {
        //findGameManager = GameObject.FindWithTag("GameManager");
        findSoundManager = GameObject.FindWithTag("SoundManager");
        findBattleManager = GameObject.FindWithTag("BattleManager");
        target = GameObject.FindWithTag("Player");

        //gameManager = findGameManager.GetComponent<GameManager>();
        battleManager = findBattleManager.GetComponent<BattleManager>();
        soundManager = findSoundManager.GetComponent<SoundManager>();
        plaSta = target.GetComponent<Player_Status_Controller>();
        myAgent = GetComponent<NavMeshAgent>();
        animCon = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        defaltAgentSpeed = myAgent.speed;
        forwardForce = forwardForce + (forwardForce * Time.deltaTime);//Time.deltaTimeを掛けることで、フレームレートで影響を出さない。

    }


    // Update is called once per frame
    void Update()
    { 
        if (enemyDie) return;

        myAgent.SetDestination(target.transform.position);

        var targetPos = target.transform.position;//ターゲットのポジション

        var enemyPos = this.transform.position;

        var distanceToTarget = Vector3.Distance(enemyPos, targetPos);

        if (lookAtPlayer)
        {
            gameObject.transform.DOLookAt(targetPos,0.1f);
        }

        if (distanceToTarget < stoppingDistance && stopping == false)
        {
            actionType = ActionType.Attack;
            myAgent.speed = 0.1f;
        }
        else if(stopping == false)
        {
            actionType = ActionType.Move;
            myAgent.speed = defaltAgentSpeed;
        }

        if (enemyHp <= 0)
        {
            EnemyOnDie();
        }

        switch (actionType)
        { 
        case ActionType.Move:

                if (AttackCollider.enabled == true)
                {
                    OffEnemyAttack();
                }

        break;

        case ActionType.Attack:

                if (attackCoolTime >= 0)
                {
                    attackCoolTime -= Time.deltaTime;
                }
                else
                { 
                    animCon.SetTrigger("EnemyAttack");
                    
                  

                    attackCoolTime = return_attackCoolTime;
                }
        break;

        case ActionType.Stop:

                if (stopCoolTime >= 0)
                {
                    stopCoolTime -= Time.deltaTime;
                }
                else
                {
                    stopping = false;
                }

        break;

        }
    }
    private void FixedUpdate()
    {
        if (enemyDie) return;

        if (actionType == ActionType.Move)
        {
            animCon.SetBool("Move", true);
        }
        else
        {
            animCon.SetBool("Move", false);
        }

      
        
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon"&&!hitting)
        {
            OnEnemyHit();

            if (enemyHitBarrierShield > 0)
            {
                animCon.SetFloat("Shield", enemyHitBarrierShield);
            }
        }
        
    }

    public void OnEnemyAttack()
    { 
        AttackCollider.enabled = true;
    }

    public void OffEnemyAttack()
    {
        AttackCollider.enabled = false;
    }

    public void OnEnemyHit()
    {
            enemyDamage = battleManager.DamegeCalculation(enemyDefence,plaSta.PlayerAttackPower);//ダメージの計算結果をダメージ変数に代入。

            enemyHp -= enemyDamage;//HP - ダメージ

            battleManager.DamageText(enemyThisCollider, enemyDamage,damageUIPos);//ダメージの表示。

            battleManager.Enemy_Hit_Effect(gameObject);

            soundManager.OneShot_Player_Sound(8);

            battleManager.Enemy_Hit_Vibration();

            EnemyBreak(); 

            EnemyKnockBack();

            hitting = true;

        StartCoroutine(HittingFalse());

        //Invoke("HittingFalse",0.2f);

        IEnumerator HittingFalse()
        {
            yield return new WaitForSeconds(0.2f);

            hitting = false;
        }
           
    }

    private void EnemyBreak()
    {
        if (enemyHitBarrierShield > 0)//シールドを持っていれば
        {
            enemyHitBarrierShield--;
        }

        if (enemyHitBarrierShield == 0)//シールドを持っていなければ
        {
            rb.isKinematic = false;

            stopping = true;//ヒット中

            if (AttackCollider.enabled == true)
            {
                OffEnemyAttack();
            }

            animCon.SetTrigger("Hit");

            actionType = ActionType.Stop;

            StopCoolSet(setStopCoolTime);

            Invoke("kinematicTrue", 0.2f);
        }   
    }

    private void EnemyOnDie()
    {
        myAgent.isStopped = true;

        //plasta.PlayerExp += getExp;

        battleManager.GetExp(target.GetComponent<CharacterController>(),getExp);

        OffCollider(); 

        animCon.SetBool("Die",true);

        StartCoroutine(EnemyDieEffectAndDestroy());

        enemyDie = true;

        IEnumerator EnemyDieEffectAndDestroy()
        {
            yield return new WaitForSeconds(2.5f);

            battleManager.Enemy_Die_Effect(gameObject);

            soundManager.OneShot_Enemy_Action_Sound(1);

            Destroy(gameObject, 0.1f);

        }
    }

   

    private void OffCollider()
    {
        enemyThisCollider.enabled = false;

        enemyThisTriggerCollider.enabled = false;

        AttackCollider.enabled = false;

    }

    //void HittingFalse()
    //{ 
    
    //}
    
    void kinematicTrue()
    {
        rb.isKinematic = true;
    }


    private void EnemyKnockBack()
    {
        Vector3 vec = transform.forward * forwardForce;
        rb.AddForce(vec, ForceMode.Impulse);
    }

    private void StopCoolSet(float SetCool)
    {

        if (stopCoolTime != SetCool)
        {
            stopCoolTime = SetCool;
        } 
    }
}
