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
    private float forwardForce;//�G���U�����󂯂��ۂ̃m�b�N�o�b�N��

    private float defaltAgentSpeed;//NavAgent�̃X�s�[�h�̃f�t�H���g�̒l

    [SerializeField]
    private float attackCoolTime = 3;  
    
    [SerializeField]
    private float return_attackCoolTime = 5f;

    private float stopCoolTime = 0;

    [SerializeField] 
    private float setStopCoolTime;

    private bool approached = false;//�v���C���[�ɋ߂Â���ON

    private bool hitting = false;//�_���[�W���󂯂��ON�B���i�q�b�g�h�~�p

    private bool stopping = false;//�_���[�W���󂯂��ON�B�_���[�W��̃N�[���_�E���p

    private bool enemyDie = false;//HP��0�ɂȂ��ON


    [SerializeField]
    private Player_Status_Controller psc;  //plasta�@>> ���̕ϐ��̖��O����������B�@���߂� psc �Ƃ��ɂ��悤�B
  
    [SerializeField] 
    private NavMeshAgent myAgent;

    private Animator animCon;

    [SerializeField] 
    private CapsuleCollider enemyThisCollider;//�G�̕�������̃R���C�_�[

    [SerializeField] 
    private BoxCollider enemyThisTriggerCollider;//�G�̓����蔻��̃R���C�_�[

    [SerializeField] 
    private Collider AttackCollider;//�G�̍U������̃R���C�_�[

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
        forwardForce = forwardForce + (forwardForce * Time.deltaTime);//Time.deltaTime���|���邱�ƂŁA�t���[�����[�g�ŉe�����o���Ȃ��B
    }


    // Update is called once per frame
    void Update()
    { 
        if (enemyDie) return;


        //�����͊ȒP�ɂǂ������������Ă��邩���������܂��傤

        myAgent.SetDestination(target.transform.position);

        var targetPos = target.transform.position;//�^�[�Q�b�g�̃|�W�V����

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
/// �v���C���[�̍U���������������̏���
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
/// �����̐����������܂��傤
/// </summary>
    public void OnEnemyAttack()
    { 
        AttackCollider.enabled = true;
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    public void OffEnemyAttack()
    {
        AttackCollider.enabled = false;
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    public void OnEnemyHit()
    {
            enemyDamage = GM.DamegeCalculation(enemyDefence,psc.PlayerAttackPower);//�_���[�W�̌v�Z���ʂ��_���[�W�ϐ��ɑ���B

            enemyHp -= enemyDamage;//HP - �_���[�W

            GM.DamageText(enemyThisCollider, enemyDamage,damageUIPos);//�_���[�W�̕\���B

            GM.Enemy_Hit_EffectAndSound(gameObject); 

            EnemyBreak(); 

            EnemyKnockBack();

            hitting = true;

        Invoke("HittingFalse",0.2f);
           
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
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

/// <summary>
/// �����̐����������܂��傤
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
/// �����̐����������܂��傤
/// </summary>
    IEnumerator EnemyDieEffectAndDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        GM.Enemy_Die_EffectAndSound(gameObject);
        Destroy(gameObject,0.1f);
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    private void OffCollider()
    {
        enemyThisCollider.enabled = false;
        enemyThisTriggerCollider.enabled = false;
        AttackCollider.enabled = false;
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    void HittingFalse()
    { 
        hitting = false;
    }
    
/// <summary>
/// �����̐����������܂��傤
/// </summary>
    void KinematicTrue()
    {
        rb.isKinematic = true;
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    private void EnemyKnockBack()
    {
        Vector3 vec = transform.forward * forwardForce;
        rb.AddForce(vec, ForceMode.Impulse);
    }

/// <summary>
/// �����̐����������܂��傤
/// </summary>
    private void StopCoolSet(float SetCool)
    {

        if (stopCoolTime != SetCool)
        {
            stopCoolTime = SetCool;
        } 
    }
}
