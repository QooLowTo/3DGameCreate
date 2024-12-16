using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの経験値を管理するクラスです。
/// </summary>
public class Player_Experience_Manager : MonoBehaviour
{
    private int exp = 0;
    private int needExp = 0;

 
    [SerializeField]
    private AnimationCurve needExpManage;

    [SerializeField]
    private Collider myCol;

    [SerializeField] 
    private Player_Status_Controller playerSCon;

    [SerializeField]
    private HealthBarScript playerHealth;
    [SerializeField]
    private GameObject findHealth;

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject findGM;

    [SerializeField]
    private StatusDate statusData;

    [SerializeField] 
    private ExpManager expManager;

    //int level = 1;
    void Start()
    {
        playerSCon = gameObject.GetComponent<Player_Status_Controller>();
        playerHealth = findHealth.GetComponent<HealthBarScript>();
        gameManager = findGM.GetComponent<GameManager>();
        statusData = GetComponent<StatusDate>();
        expManager = GetComponent<ExpManager>();

        //elementNum = statusDate.D_ExpListElement;
    }

    void Update()
    {
      
      //以下の処理の説明を簡単に書いてください、（どういう場合に発生するかなど）
        if (playerSCon.PlayerExp >= expManager.ExpTablesList[statusData.D_ExpListElement] && playerSCon.PlayerLevel != 99)
        {
            LevelUp();
        }

        //if (level == 99) return;
        //level++;
        //Debug.Log($"level:{level} next:{(int)needExpManage.Evaluate(level)}");
        //expManager.ExpTablesList.Add((int)needExpManage.Evaluate(level));
    }

/// <summary>
/// レベルアップ時の処理を行うメソッドです。
/// </summary>
    public void LevelUp()
    {
        playerSCon.PlayerLevel++;
        playerSCon.PlayerHP += expManager.HpGrowthTableList[statusData.D_ExpListElement];
        playerSCon.PlayerAttackPower += expManager.AttackPGrowthTableList[statusData.D_ExpListElement];
        playerSCon.PlayerDefance += expManager.DefanceGrowthTableList1[statusData.D_ExpListElement];

        playerSCon.LivePlayerHP = playerSCon.PlayerHP;
        playerHealth.SetMaxHealth(playerSCon.PlayerHP);
        gameManager.Player_LevelUp_EffectAndSound(myCol,gameObject);
        
        statusData.D_ExpListElement++;
    }
}
