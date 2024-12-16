using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�̓G�̍U���𐧌䂷��N���X�ł��B
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    private int playerHitDamage;

    [SerializeField]
    private int enemyAttackPower;

    [SerializeField] 
    private Player_Battle_Controller pbc;
    [SerializeField] 
    private Player_Status_Controller psc;

    [SerializeField] private GameManager gameManager;
    private GameObject manager; //���������܂��傤���Ƃ����GameManager��T�����߂ɕK�v�ȃQ�[���I�u�W�F�N�g�Ȃ�managerGO�Ƃ��ɂ��悤
    private GameObject playerObj;�@//�����Player�ɂ��Ă���OBJ�Ȃ�AplayerObject�ɂ��Ă�������

    private bool attacking = false;
    // Start is called before the first frame update
    void Awake()
    { 
        manager = GameObject.FindWithTag("GameManager"); 
        playerObj = GameObject.FindWithTag("Player");
       
       
        pbc = playerObj.GetComponent<Player_Battle_Controller>();
        psc = playerObj.GetComponent <Player_Status_Controller>();
       
        gameManager = manager.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObj && pbc.Avoidancing == false&& pbc.IsDown == false&& attacking == false)
        {
            
            EnemyAttackHit();
        }
    }

/// <summary>
/// �v���C���[�ɍU���𓖂Ă郁�\�b�h
/// </summary>
    private void EnemyAttackHit()
    { 
        pbc.OnHit();
        pbc.AnimCon.SetTrigger("Hit");
        gameManager.Player_Damage_Sound();
        playerHitDamage = gameManager.DamegeCalculation(psc.PlayerDefance,enemyAttackPower);
        psc.LivePlayerHP -= playerHitDamage;
        gameManager.DamageText(playerObj.GetComponent<CharacterController>(),playerHitDamage,0.2f);//�}�W�b�N�i���o�[���� 0.2f�͕ϐ��ɂ��Ă�������  

        attacking = true;
        Invoke("AttackingFalse", 0.2f); //�}�W�b�N�i���o�[���� 0.2f�͕ϐ��ɂ��Ă�������    
    }

/// <summary>
/// �U�����̃t���O��false�ɂ��郁�\�b�h
/// </summary>
    private void AttackingFalse()
    { 
        attacking = false;
    }
}
