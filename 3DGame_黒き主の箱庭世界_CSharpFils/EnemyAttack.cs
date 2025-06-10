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

  
    private Player_Battle_Controller playerBattleCon;

    private GameObject target;
    private Player_Status_Controller playerStatCon;

    private BattleManager battleManager;
    private GameObject findManager;

    private bool attaking = false;
   
    void Start()
    {

        findManager = GameObject.FindWithTag("BattleManager");
        target = GameObject.FindWithTag("Player");

        playerBattleCon = target.GetComponent<Player_Battle_Controller>();
        playerStatCon = target.GetComponent<Player_Status_Controller>();

        battleManager = findManager.GetComponent<BattleManager>();
    }

    /// <summary>
    /// //説明書いて
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != target || playerBattleCon.Avoidancing || playerBattleCon.Hitting || playerBattleCon.IsDown || attaking) return;

        EnemyAttackHit();
    }

    /// <summary>
    /// //説明書いて
    /// </summary>
    private void EnemyAttackHit()
    {
        playerHitDamage = battleManager.DamegeCalculation(playerStatCon.PlayerDefance, enemyAttackPower);

        playerBattleCon.OnHit(playerHitDamage);

    }

}
