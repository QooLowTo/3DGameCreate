using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのゴーレムの攻撃を制御するクラスです。
/// </summary>
public class BossGolem_Attack : MonoBehaviour
{

    private Player_Battle_Controller playerController;

    private BossGolem bossGolem;

    private int playerHitDamage;

    [SerializeField, Header("ボスの通常攻撃の威力")]
    private int bossAttackDamage;

    [SerializeField, Header("ボスのノックバック攻撃の威力")]
    private int bossKnockBackAttackDamage;

    private bool isAttacking = false;//攻撃中かどうか判定。重複してヒットしないよう

    [SerializeField, Header("ボスのノックバック量")]
    private float knockbackPower = 30f;

    [SerializeField, Header("ボスのノックバック間隔")]
    private float knockbackSpeed = 0.5f;

    void Start()
    {
        bossGolem = GetComponent<BossGolem>();

        playerController = bossGolem.Target.GetComponent<Player_Battle_Controller>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossGolem.BossAttackActionType() == "" || other.gameObject != bossGolem.Target || playerController.IsDown || playerController.Avoidancing || isAttacking) return;

        Debug.Log(bossGolem.BossAttackActionType());

        BossAttack();

    }

     void BossAttack()
        {

            if ((bossGolem.BossAttackActionType() == "Attack")){

                playerHitDamage = bossGolem.BattleManager.DamegeCalculation(bossGolem.PlaSta.PlayerDefance, bossAttackDamage);

                playerController.OnHit(playerHitDamage);

                isAttacking = true;

                StartCoroutine(AttackingFalse());

                IEnumerator AttackingFalse()
                {
                    yield return new WaitForSeconds(0.2f);

                    isAttacking = false;
                }

            }else{

                playerHitDamage = bossGolem.BattleManager.DamegeCalculation(bossGolem.PlaSta.PlayerDefance, bossAttackDamage);

                playerController.OnKnockBack(playerHitDamage, knockbackPower, knockbackSpeed,gameObject.transform);

            }
        }
}
