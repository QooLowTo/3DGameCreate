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

  
    private Player_Battle_Controller placon;


    private GameObject target;
    private Player_Status_Controller Plasta;

    private BattleManager battleManager;
    ////private GameManager gameManager;
    private GameObject findManager;
    //private GameObject findPla;

    private bool attaking = false;
    // Start is called before the first frame update
    void Start()
    {

     

        findManager = GameObject.FindWithTag("BattleManager");
        target = GameObject.FindWithTag("Player");


        placon = target.GetComponent<Player_Battle_Controller>();
        Plasta = target.GetComponent<Player_Status_Controller>();

        //gameManager = findManager.GetComponent<GameManager>();

        battleManager = findManager.GetComponent<BattleManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != target || placon.Avoidancing || placon.Hitting || placon.IsDown || attaking) return;
        
            
            EnemyAttackHit();
        
    }

    private void EnemyAttackHit()
    { 
        playerHitDamage = battleManager.DamegeCalculation(Plasta.PlayerDefance,enemyAttackPower);

        placon.OnHit(playerHitDamage);
        //Placon.AnimCon.SetTrigger("Hit");
        //gameManager.OneShotPlayerSound(6);
        
        //Plasta.LivePlayerHP -= playerHitDamage;
        //gameManager.DamageText(findPla.GetComponent<CharacterController>(),playerHitDamage,0.2f);

        //attaking = true;
        //Invoke("AttackingFalse", 0.2f);

        //StartCoroutine(AttackingfFalse());

        //IEnumerator AttackingfFalse()
        //{

        //    yield return new WaitForSeconds(0.2f);
        //    attaking = false;
        //}
    }

}
