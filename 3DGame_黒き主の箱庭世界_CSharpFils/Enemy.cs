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
    private int enemyHitBarrierShield; //説明書いて 何のための変数？

    [SerializeField, Header("敵の経験値")]
    protected int getExp; //敵を倒した際にプレイヤーが得る経験値 -> 合っている？


    protected int enemyDamage; //説明書いて

    [SerializeField, Header("ダメージ表記のポジション")]
    protected float damageUIPos = 0.2f;

    [SerializeField]
    private float stoppingDistance; //説明書いて
  
    [SerializeField,Header("正面方向の力")] 
    protected float forwardForce;//敵が攻撃を受けた際のノックバック量

    private float defaultAgentSpeed;//NavAgentのスピードのデフォルトの値

    [SerializeField]
    private float attackCoolTime = 3;  
    
    [SerializeField]
    private float return_attackCoolTime = 5f; //なんのリターン？？

    private float stopCoolTime = 0;

    [SerializeField] 
    private float setStopCoolTime;

    [SerializeField]
    private bool lookAtPlayer = false;

    private bool approached = false;//プレイヤーに近づくとON

    private bool hitting = false;//ダメージを受けるとON。多段ヒット防止用

    private bool stopping = false;//ダメージを受けるとON。ダメージ後のクールダウン用

    private bool enemyDie = false; //HPが０になったらtrueになる。敵の死亡フラグ


    protected Player_Status_Controller playerStatCon; //他のクラスと統一してください
  
    protected NavMeshAgent myAgent;

    protected Animator animator;

    protected Rigidbody rb;

    [SerializeField] 
    private CapsuleCollider enemyCollider;//敵の物理判定のコライダー

    [SerializeField] 
    private BoxCollider enemyTriggerCollider;//敵の当たり判定のコライダー

    [SerializeField] 
    private Collider attackCollider;//敵の攻撃判定のコライダー

    protected SoundManager soundManager;

    protected BattleManager battleManager;

    protected GameObject findSoundManager;

    protected GameObject findBattleManager;
      
    protected GameObject target; 

    private enum ActionType
    { 
     Move,
     Stop,
     Attack
    }ActionType actionType;

    public bool EnemyDie { get => enemyDie; set => enemyDie = value; }
    public int EnemyHp { get => enemyHp; set => enemyHp = value; }
    public GameObject Target { get => target; set => target = value; }
    public Player_Status_Controller PlaSta { get => playerStatCon; set => playerStatCon = value; }
    public BattleManager BattleManager { get => battleManager; set => battleManager = value; }

    void Start()
    {  
        StartEnemySetting();
    }

    protected void StartEnemySetting()
    {
        //このブロックの中どういう処理をやっているのか、簡単に説明を残してください
        findSoundManager = GameObject.FindWithTag("SoundManager");
        findBattleManager = GameObject.FindWithTag("BattleManager");
        target = GameObject.FindWithTag("Player");

        battleManager = findBattleManager.GetComponent<BattleManager>();
        soundManager = findSoundManager.GetComponent<SoundManager>();
        playerStatCon = target.GetComponent<Player_Status_Controller>();
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        defaultAgentSpeed = myAgent.speed;
        forwardForce = forwardForce + (forwardForce * Time.deltaTime);　//Time.deltaTimeを掛けることで、フレームレートで影響を出さない。

    }


    void Update()
    { 
        if (enemyDie) return;

        myAgent.SetDestination(target.transform.position);

        var targetPos = target.transform.position;　//ターゲットのポジション

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
            myAgent.speed = defaultAgentSpeed;
        }

        if (enemyHp <= 0)
        {
            EnemyOnDie();
        }

        switch (actionType)
        { 
        case ActionType.Move:

                if (attackCollider.enabled == true)
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
                    animator.SetTrigger("EnemyAttack");
                    
                  

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
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon"&&!hitting)
        {
            OnEnemyHit();

            if (enemyHitBarrierShield > 0)
            {
                animator.SetFloat("Shield", enemyHitBarrierShield);
            }
        }
        
    }

    public void OnEnemyAttack()
    { 
        attackCollider.enabled = true;
    }

    public void OffEnemyAttack()
    {
        attackCollider.enabled = false;
    }

    public void OnEnemyHit()
    {
            enemyDamage = battleManager.DamegeCalculation(enemyDefence,playerStatCon.PlayerAttackPower);//ダメージの計算結果をダメージ変数に代入。

            enemyHp -= enemyDamage;//HP - ダメージ

            battleManager.DamageText(enemyCollider, enemyDamage,damageUIPos);//ダメージの表示。

            battleManager.Enemy_Hit_Effect(gameObject);

            //マジックナンバーを避けるために、変数に置き換えることを検討してください・・
            soundManager.OneShot_Player_Sound(8);

            battleManager.Enemy_Hit_Vibration();

            EnemyBreak(); 

            EnemyKnockBack();

            hitting = true;

        StartCoroutine(HittingFalse());

        IEnumerator HittingFalse()
        {
            //0.2f秒後延長 -> これでもいいが、変数化をおすすめします
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

            if (attackCollider.enabled == true)
            {
                OffEnemyAttack();
            }

            animator.SetTrigger("Hit");

            actionType = ActionType.Stop;

            StopCoolSet(setStopCoolTime);

            //マジックナンバー
            Invoke("kinematicTrue", 0.2f);
        }   
    }

    private void EnemyOnDie()
    {
        myAgent.isStopped = true;


        battleManager.GetExp(target.GetComponent<CharacterController>(),getExp);

        OffCollider(); 

        animator.SetBool("Die",true);

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
        enemyCollider.enabled = false;

        enemyTriggerCollider.enabled = false;

        attackCollider.enabled = false;

    }

    /// <summary>
    /// //説明書いて
    /// </summary>
    void KinematicTrue()
    {
        rb.isKinematic = true;
    }


    /// <summary>
    /// //説明書いて
    /// </summary>
    private void EnemyKnockBack()
    {
        Vector3 vec = transform.forward * forwardForce;
        rb.AddForce(vec, ForceMode.Impulse);
    }

    /// <summary>
    /// //説明書いて
    /// </summary>
    /// <param name="SetCool"></param>
    private void StopCoolSet(float SetCool)
    {

        if (stopCoolTime != SetCool)
        {
            stopCoolTime = SetCool;
        }
    }
}
