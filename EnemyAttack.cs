using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドの敵の攻撃を制御するクラスです。
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
    private GameObject manager; //説明書きましょうあとこれはGameManagerを探すために必要なゲームオブジェクトならmanagerGOとかにしよう
    private GameObject playerObj;　//これはPlayerについているOBJなら、playerObjectにしてください

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
/// プレイヤーに攻撃を当てるメソッド
/// </summary>
    private void EnemyAttackHit()
    { 
        pbc.OnHit();
        pbc.AnimCon.SetTrigger("Hit");
        gameManager.Player_Damage_Sound();
        playerHitDamage = gameManager.DamegeCalculation(psc.PlayerDefance,enemyAttackPower);
        psc.LivePlayerHP -= playerHitDamage;
        gameManager.DamageText(playerObj.GetComponent<CharacterController>(),playerHitDamage,0.2f);//マジックナンバー発見 0.2fは変数にしてください  

        attacking = true;
        Invoke("AttackingFalse", 0.2f); //マジックナンバー発見 0.2fは変数にしてください    
    }

/// <summary>
/// 攻撃中のフラグをfalseにするメソッド
/// </summary>
    private void AttackingFalse()
    { 
        attacking = false;
    }
}
