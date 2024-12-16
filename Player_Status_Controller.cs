using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータスを管理するクラスです。
/// </summary>
public class Player_Status_Controller : MonoBehaviour
{
    [SerializeField]
    private int playerLevel = 1;
    [SerializeField]
    private int playerExp = 0;
    
    [SerializeField] 
    private int playerHP = 50;
    [SerializeField]
    private int livePlayerHP;

    [SerializeField] 
    private int playerAttackPower = 3;
    [SerializeField]
    private int playerDefence = 1;

    [SerializeField]
    private HealthBarScript playerHealth;
    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private StatusDate statusData;


    //---プロパティ---//
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
    public int PlayerExp { get => playerExp; set => playerExp = value; }
    public int PlayerHP { get => playerHP; set => playerHP = value; }
    public int PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = value; }
    public int PlayerDefence { get => playerDefence; set => playerDefence = value; }
    public int LivePlayerHP { get => livePlayerHP; set => livePlayerHP = value; }
    public GameObject HealthBar { get => healthBar; set => healthBar = value; }
  
    void Start()
    {
        //if (playerLevel == StatusDate.D_PlayerLevel) return;

        playerLevel = statusData.D_PlayerLevel;

        playerExp = statusData.D_PlayerExp;

        playerHP = statusData.D_PlayerHP;

        playerAttackPower = statusData.D_PlayerAttackPower;

        playerDefence = statusData.D_PlayerDefance;

        livePlayerHP = statusData.D_LivePlayerHP;

        playerHealth = healthBar.GetComponent<HealthBarScript>();

       playerHealth.SetMaxHealth(PlayerHP);
    }

    void Update()
    {
        if (playerHealth != null)
        { 
             playerHealth.SetHealth(livePlayerHP);
        }
        
        if (livePlayerHP < 0)
        {
            playerHealth.HealthDeath();
            livePlayerHP = 0;     
        }
    }
}
