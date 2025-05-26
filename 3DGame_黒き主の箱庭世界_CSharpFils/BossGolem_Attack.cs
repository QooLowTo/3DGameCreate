using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのゴーレムの攻撃を制御するクラスです。
/// </summary>
public class BossGolem_Attack : MonoBehaviour
{
   
    private Player_Battle_Controller plaCon;

    //private Player_Status_Controller Plasta;

    //private GameManager gameManager;

    private BossGolem bossGolem;

    private int playerHitDamage;

    [SerializeField,Header("ボスの通常攻撃の威力")]
    private int bossAttackDamage;

    [SerializeField,Header("ボスのノックバック攻撃の威力")]
    private int bossKnockBackAttackDamage;

    private bool attaking = false;//重複してヒットしないよう

    [SerializeField,Header("ボスのノックバック量")] 
    private float knockbackPower = 30f;

    [SerializeField,Header("ボスのノックバック間隔")] 
    private float knockbackSpeed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        bossGolem = GetComponent<BossGolem>();
        //StartEnemySetting();

        //findPla = GameObject.FindWithTag("Player");

        //findGM = GameObject.FindWithTag("GameManager");

        plaCon = bossGolem.Target.GetComponent<Player_Battle_Controller>();

        //Plasta = findPla.GetComponent<Player_Status_Controller>();

       

        //gameManager = findGM.GetComponent<GameManager>();
       
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (bossGolem.BossAttackActionType() == "" || other.gameObject != bossGolem.Target || plaCon.IsDown || plaCon.Avoidancing || attaking) return;

        Debug.Log(bossGolem.BossAttackActionType());

        //switch (bossGolem.BossAttackActionType()){

        //    case "Attack":

        //        //other = bossGolem.AttackCollider;

        //        BossAttack();

        //        break;

        //    case "HornAttack":

        //        //other = bossGolem.HornAttackCollider;

        //        BossAttack();

        //        break;

        //    case "Impact":

        //        other = bossGolem.ImpactCollider;

        //    break;
           
        //}

        //if (other.gameObject == findPla && !plaCon.IsDown)
        //{

            BossAttack();

        //}

        void BossAttack()
        {

            if ((bossGolem.BossAttackActionType() == "Attack")){

                playerHitDamage = bossGolem.BattleManager.DamegeCalculation(bossGolem.PlaSta.PlayerDefance, bossAttackDamage);

                plaCon.OnHit(playerHitDamage);

                attaking = true;

                StartCoroutine(AttackingFalse());

                IEnumerator AttackingFalse()
                {
                    yield return new WaitForSeconds(0.2f);

                    attaking = false;
                }

            }else{

                playerHitDamage = bossGolem.BattleManager.DamegeCalculation(bossGolem.PlaSta.PlayerDefance, bossAttackDamage);

                plaCon.OnKnockBack(playerHitDamage, knockbackPower, knockbackSpeed,gameObject.transform);

            }
        }
    }
}
