using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status_Controller : MonoBehaviour
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

    private HealthBarScript plaHealth;
    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private StatusDate statusDate;


    //---プロパティ---//
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
    public int PlayerExp { get => playerExp; set => playerExp = value; }
    public int PlayerHP { get => playerHP; set => playerHP = value; }
    public int PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = value; }
    public int PlayerDefance { get => playerDefance; set => playerDefance = value; }
    public int LivePlayerHP { get => livePlayerHP; set => livePlayerHP = value; }
    public GameObject HealthBar { get => healthBar; set => healthBar = value; }
  


    // Start is called before the first frame update
    void Start()
    {
        //if (playerLevel == StatusDate.D_PlayerLevel) return;

        playerLevel = statusDate.D_PlayerLevel;

        playerExp = statusDate.D_PlayerExp;

        playerHP = statusDate.D_PlayerHP;

        playerAttackPower = statusDate.D_PlayerAttackPower;

        playerDefance = statusDate.D_PlayerDefance;

        livePlayerHP = statusDate.D_LivePlayerHP;

        plaHealth = healthBar.GetComponent<HealthBarScript>();

        plaHealth.SetMaxHealth(PlayerHP);

    }

    // Update is called once per frame
    //void Update()
    //{
      
        
      
    //}

    /// <summary>
    /// 被ダメージ時やレベルアップ時などで値を更新するメソッド。HPは0以下にはならないように制御しています。
    /// </summary>
    public void SetLiveHP()
    {
        if (plaHealth != null){

            plaHealth.SetHealth(livePlayerHP);


            if (livePlayerHP < 0)
            {
                livePlayerHP = 0;

                plaHealth.SetHealth(livePlayerHP);

                plaHealth.HealthDeath();

            }
        }

       
    }
}
