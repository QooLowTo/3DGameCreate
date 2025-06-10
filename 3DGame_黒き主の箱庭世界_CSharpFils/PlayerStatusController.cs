using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのステータスを管理するクラスです。
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    [SerializeField,Header("レベル")]
    private int playerLevel = 1;
    [SerializeField, Header("経験値")]
    private int playerExp = 0;
    
    [SerializeField, Header("最大HP")] 
    private int playerHP = 50;
    [SerializeField, Header("現在HP")]
    private int livePlayerHP;

    [SerializeField, Header("攻撃")] 
    private int playerAttackPower = 3;
    [SerializeField, Header("防御力")]
    private int playerDefance = 1;

    private HealthBarScript playerHealthBarScript;

    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private StatusDate statusData;


    //---プロパティ---//
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
    public int PlayerExp { get => playerExp; set => playerExp = value; }
    public int PlayerHP { get => playerHP; set => playerHP = value; }
    public int PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = value; }
    public int PlayerDefance { get => playerDefance; set => playerDefance = value; }
    public int LivePlayerHP { get => livePlayerHP; set => livePlayerHP = value; }
    public GameObject HealthBar { get => healthBar; set => healthBar = value; }
  
    void Start()
    {

        playerLevel = statusData.D_PlayerLevel;

        playerExp = statusData.D_PlayerExp;

        playerHP = statusData.D_PlayerHP;

        playerAttackPower = statusData.D_PlayerAttackPower;

        playerDefance = statusData.D_PlayerDefance;

        livePlayerHP = statusData.D_LivePlayerHP;

        playerHealthBarScript = healthBar.GetComponent<HealthBarScript>();

        playerHealthBarScript.SetMaxHealth(PlayerHP);

    }

   
    /// <summary>
    /// 被ダメージ時やレベルアップ時などで値を更新するメソッド。HPは0以下にはならないように制御しています。
    /// </summary>
    public void SetLiveHP()
    {
        if (playerHealthBarScript != null){

            playerHealthBarScript.SetHealth(livePlayerHP);


            if (livePlayerHP < 0)
            {
                livePlayerHP = 0;

                playerHealthBarScript.SetHealth(livePlayerHP);

                playerHealthBarScript.HealthDeath();

            }
        }
       
    }
}
