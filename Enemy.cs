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
    [SerializeField] 
    private int enemyHp;

    [SerializeField] 
    private int enemyDefence;

    [SerializeField] 
    private int enemyHitBarrierShield;

    [SerializeField]
    private int getExp;


    private int enemyDamage;

    [SerializeField] 
    private float damageUIPos = 0.2f;

    [SerializeField]
    private float stoppingDistance;
  
    [SerializeField] 
    private float forwardForce;//敵が攻撃を受けた際のノックバック量

    private float defaltAgentSpeed;//NavAgentのスピードのデフォルトの値

    [SerializeField]
    private float attackCoolTime = 3;  
    
    [SerializeField]
    private float return_attackCoolTime = 5f;

    private float stopCoolTime = 0;

    [SerializeField] 
    private float setStopCoolTime;

    private bool approached = false;//プレイヤーに近づくとON

    private bool hitting = false;//ダメージを受けるとON。多段ヒット防止用

    private bool stopping = false;//ダメージを受けるとON。ダメージ後のクールダウン用

    private bool enemyDie = false;//HPが0になるとON


    [SerializeField]
    private Player_Status_Controller psc;  //plasta　>> この変数の名前おかしいよ。　せめて psc とかにしよう。
  
    [SerializeField] 
    private NavMeshAgent myAgent;

    private Animator animCon;

    [SerializeField] 
    private CapsuleCollider enemyThisCollider;//敵の物理判定のコライダー

    [SerializeField] 
    private BoxCollider enemyThisTriggerCollider;//敵の当たり判定のコライダー

    [SerializeField] 
    private Collider AttackCollider;//敵の攻撃判定のコライダー

    [SerializeField] 
    private Rigidbody rb;


    [SerializeField] 
    private GameManager GM; 

    private GameObject FindGameManager;
      
    private GameObject target; 
    // Start is called before the first frame update

    public enum ActionType
    { 
     Move,
     Stop,
     Attack
    }ActionType actionType;

    public bool EnemyDie { get => enemyDie; set => enemyDie = value; }
    public int EnemyHp { get => enemyHp; set => enemyHp = value; }

    void Start()
    {  
        FindGameManager = GameObject.FindWithTag("GameManager");
        target = GameObject.FindWithTag("Player");

        GM = FindGameManager.GetComponent<GameManager>();
        psc = target.GetComponent<Player_Status_Controller>();
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


        //ここは簡単にどういう処理しているか説明書きましょう

        myAgent.SetDestination(target.transform.position);

        var targetPos = target.transform.position;//ターゲットのポジション

        var enemyPos = this.transform.position;

        var distanceToTarget = Vector3.Distance(enemyPos, targetPos);

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

        if (enemyHitBarrierShield > 0)
        { 
        animCon.SetFloat("Shield",enemyHitBarrierShield);
        }
        
       
    }

/// <summary>
/// プレイヤーの攻撃が当たった時の処理
/// </summary>
/// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon"&&hitting == false)
        {
            OnEnemyHit();
        }
        
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    public void OnEnemyAttack()
    { 
        AttackCollider.enabled = true;
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    public void OffEnemyAttack()
    {
        AttackCollider.enabled = false;
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    public void OnEnemyHit()
    {
            enemyDamage = GM.DamegeCalculation(enemyDefence,psc.PlayerAttackPower);//ダメージの計算結果をダメージ変数に代入。

            enemyHp -= enemyDamage;//HP - ダメージ

            GM.DamageText(enemyThisCollider, enemyDamage,damageUIPos);//ダメージの表示。

            GM.Enemy_Hit_EffectAndSound(gameObject); 

            EnemyBreak(); 

            EnemyKnockBack();

            hitting = true;

        Invoke("HittingFalse",0.2f);
           
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
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

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    private void EnemyOnDie()
    {
        myAgent.isStopped = true; 
        GM.GetExpText(enemyThisCollider,getExp);
        psc.PlayerExp += getExp;
        OffCollider(); 
        animCon.SetBool("Die",true);
        StartCoroutine(EnemyDieEffectAndDestroy());
        enemyDie = true;        
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    IEnumerator EnemyDieEffectAndDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        GM.Enemy_Die_EffectAndSound(gameObject);
        Destroy(gameObject,0.1f);
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    private void OffCollider()
    {
        enemyThisCollider.enabled = false;
        enemyThisTriggerCollider.enabled = false;
        AttackCollider.enabled = false;
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    void HittingFalse()
    { 
        hitting = false;
    }
    
/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    void KinematicTrue()
    {
        rb.isKinematic = true;
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    private void EnemyKnockBack()
    {
        Vector3 vec = transform.forward * forwardForce;
        rb.AddForce(vec, ForceMode.Impulse);
    }

/// <summary>
/// 処理の説明を書きましょう
/// </summary>
    private void StopCoolSet(float SetCool)
    {

        if (stopCoolTime != SetCool)
        {
            stopCoolTime = SetCool;
        } 
    }
}
