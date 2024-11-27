using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int playerDefance = 1;

    [SerializeField]
    private HealthBarScript plaHealth;
    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private StatusDate StatusDate;


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

        playerLevel = StatusDate.D_PlayerLevel;

        playerExp = StatusDate.D_PlayerExp;

        playerHP = StatusDate.D_PlayerHP;

        playerAttackPower = StatusDate.D_PlayerAttackPower;

        playerDefance = StatusDate.D_PlayerDefance;

        livePlayerHP = StatusDate.D_LivePlayerHP;

        plaHealth = healthBar.GetComponent<HealthBarScript>();

       plaHealth.SetMaxHealth(PlayerHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (plaHealth != null)
        { 
        plaHealth.SetHealth(livePlayerHP);
        }
        
        if (livePlayerHP < 0)
        {
            plaHealth.HealthDeath();
            livePlayerHP = 0;     
        }
    }
}
