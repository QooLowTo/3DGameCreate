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
    private Player_Battle_Controller Placon;
    [SerializeField] 
    private Player_Status_Controller Plasta;

    [SerializeField] private GameManager GameManager;
    private GameObject Manager;
    private GameObject PlaOb;

    private bool attaking = false;
    // Start is called before the first frame update
    void Awake()
    { 
        Manager = GameObject.FindWithTag("GameManager"); 
        PlaOb = GameObject.FindWithTag("Player");
       
       
        Placon = PlaOb.GetComponent<Player_Battle_Controller>();
        Plasta = PlaOb.GetComponent <Player_Status_Controller>();
       
        GameManager = Manager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlaOb && Placon.Avoidancing == false&& Placon.IsDown == false&& attaking == false)
        {
            
            EnemyAttackHit();
        }
    }

    private void EnemyAttackHit()
    { 
        Placon.OnHit();
        Placon.AnimCon.SetTrigger("Hit");
        GameManager.Player_Damage_Sound();
        playerHitDamage = GameManager.DamegeCalculation(Plasta.PlayerDefance,enemyAttackPower);
        Plasta.LivePlayerHP -= playerHitDamage;
        GameManager.DamageText(PlaOb.GetComponent<CharacterController>(),playerHitDamage,0.2f);

        attaking = true;
        Invoke("AttackingFalse", 0.2f);
    }

    private void AttackingFalse()
    { 
    attaking = false;
    }
}
