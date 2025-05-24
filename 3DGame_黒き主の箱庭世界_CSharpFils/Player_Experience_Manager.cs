using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Experience_Manager : MonoBehaviour
{
    private int exp = 0;
    private int needExp = 0;

    
 
    //[SerializeField]
    //private AnimationCurve needExpManage;

    [SerializeField]
    private Collider myCol;

  
    private Player_Status_Controller plasta;

  
    private HealthBarScript plaHealth;
    [SerializeField]
    private GameObject findHealth;

    private BattleManager battleManager;
    [SerializeField]
    private GameObject findBattleManager;

  

    [SerializeField]
    private StatusDate statusDate;
    [SerializeField]
    private ExpManager expManager;

   
    //[SerializeField]
    //List<int> expTablesList = new List<int>();

    //[SerializeField]
    //List<int> hpGrowthTableList = new List<int>();

    //[SerializeField]
    //List<int> attackPGrowthTableList = new List<int>();

    //[SerializeField]
    //List<int> defanceGrowthTableList = new List<int>();



    //int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        plasta = gameObject.GetComponent<Player_Status_Controller>();
        plaHealth = findHealth.GetComponent<HealthBarScript>();
        if (findBattleManager != null)
        {
            battleManager = findBattleManager.GetComponent<BattleManager>();
        }
        //elementNum = statusDate.D_ExpListElement;
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
        

    //    if (plasta.PlayerExp >= expManager.ExpTablesList[statusDate.D_ExpListElement])
    //    {
    //        LevelUp();
    //    }
    //    else
    //    {
    //        return;
    //    }

    //    //if (level == 99) return;
    //    //level++;
    //    //Debug.Log($"level:{level} next:{(int)needExpManage.Evaluate(level)}");
    //    //expManager.ExpTablesList.Add((int)needExpManage.Evaluate(level));
    //}

    public void LevelUp()
    {
        plasta.PlayerLevel++;
        statusDate.D_PlayerLevel = plasta.PlayerLevel;

        plasta.PlayerHP += expManager.HpGrowthTableList[statusDate.D_ExpListElement];
        statusDate.D_PlayerHP = plasta.PlayerHP;

        plasta.PlayerAttackPower += expManager.AttackPGrowthTableList[statusDate.D_ExpListElement];
        statusDate.D_PlayerAttackPower = plasta.PlayerAttackPower;

        plasta.PlayerDefance += expManager.DefanceGrowthTableList1[statusDate.D_ExpListElement];
        statusDate.D_PlayerDefance = plasta.PlayerDefance;

        plasta.LivePlayerHP = plasta.PlayerHP;

        plaHealth.SetMaxHealth(plasta.PlayerHP);

        plaHealth.SetHealth(plasta.PlayerHP);

        battleManager.Player_LevelUp_EffectAndSound(myCol, gameObject);

       
        statusDate.D_ExpListElement++;



    }

  
}
