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
/// �t�B�[���h�̓G�𐧌䂷��N���X�ł��B
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField,Header("�G��HP")]
    protected int enemyHp;

    [SerializeField, Header("�G�̖h���")]
    protected int enemyDefence;

    [SerializeField] 
    private int enemyHitBarrierShield;

    [SerializeField, Header("�G�̌o���l")]
    protected int getExp;


    protected int enemyDamage;

    [SerializeField, Header("�_���[�W�\�L�̃|�W�V����")]
    protected float damageUIPos = 0.2f;

    [SerializeField]
    private float stoppingDistance;
  
    [SerializeField,Header("���ʕ����̗�")] 
    protected float forwardForce;//�G���U�����󂯂��ۂ̃m�b�N�o�b�N��

    private float defaltAgentSpeed;//NavAgent�̃X�s�[�h�̃f�t�H���g�̒l

    [SerializeField]
    private float attackCoolTime = 3;  
    
    [SerializeField]
    private float return_attackCoolTime = 5f;

    private float stopCoolTime = 0;

    [SerializeField] 
    private float setStopCoolTime;

    [SerializeField]
    private bool lookAtPlayer = false;

    private bool approached = false;//�v���C���[�ɋ߂Â���ON

    private bool hitting = false;//�_���[�W���󂯂��ON�B���i�q�b�g�h�~�p

    private bool stopping = false;//�_���[�W���󂯂��ON�B�_���[�W��̃N�[���_�E���p

    private bool enemyDie = false;//HP��0�ɂȂ��ON


    protected Player_Status_Controller plaSta;
  
    protected NavMeshAgent myAgent;

    protected Animator animCon;

    protected Rigidbody rb;

    [SerializeField] 
    private CapsuleCollider enemyThisCollider;//�G�̕�������̃R���C�_�[

    [SerializeField] 
    private BoxCollider enemyThisTriggerCollider;//�G�̓����蔻��̃R���C�_�[

    [SerializeField] 
    private Collider AttackCollider;//�G�̍U������̃R���C�_�[





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
        //forwardForce = forwardForce + (forwardForce * Time.deltaTime);//Time.deltaTime���|���邱�ƂŁA�t���[�����[�g�ŉe�����o���Ȃ��B
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
        forwardForce = forwardForce + (forwardForce * Time.deltaTime);//Time.deltaTime���|���邱�ƂŁA�t���[�����[�g�ŉe�����o���Ȃ��B

    }


    // Update is called once per frame
    void Update()
    { 
        if (enemyDie) return;

        myAgent.SetDestination(target.transform.position);

        var targetPos = target.transform.position;//�^�[�Q�b�g�̃|�W�V����

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
            enemyDamage = battleManager.DamegeCalculation(enemyDefence,plaSta.PlayerAttackPower);//�_���[�W�̌v�Z���ʂ��_���[�W�ϐ��ɑ���B

            enemyHp -= enemyDamage;//HP - �_���[�W

            battleManager.DamageText(enemyThisCollider, enemyDamage,damageUIPos);//�_���[�W�̕\���B

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
        if (enemyHitBarrierShield > 0)//�V�[���h�������Ă����
        {
            enemyHitBarrierShield--;
        }

        if (enemyHitBarrierShield == 0)//�V�[���h�������Ă��Ȃ����
        {
            rb.isKinematic = false;

            stopping = true;//�q�b�g��

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
