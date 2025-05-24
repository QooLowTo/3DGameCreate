using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status_Controller : MonoBehaviour
{
    [SerializeField,Header("���x��")]
    private int playerLevel = 1;
    [SerializeField, Header("�o���l")]
    private int playerExp = 0;
    
    [SerializeField, Header("�ő�HP")] 
    private int playerHP = 50;
    [SerializeField, Header("����HP")]
    private int livePlayerHP;

    [SerializeField, Header("�U��")] 
    private int playerAttackPower = 3;
    [SerializeField, Header("�h���")]
    private int playerDefance = 1;

    private HealthBarScript plaHealth;
    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private StatusDate statusDate;


    //---�v���p�e�B---//
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
    /// ��_���[�W���⃌�x���A�b�v���ȂǂŒl���X�V���郁�\�b�h�BHP��0�ȉ��ɂ͂Ȃ�Ȃ��悤�ɐ��䂵�Ă��܂��B
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
