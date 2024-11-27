using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Experience_Manager : MonoBehaviour
{
    private int exp = 0;
    private int needExp = 0;

 
    [SerializeField]
    private AnimationCurve needExpManage;

    [SerializeField]
    private Collider myCol;

    [SerializeField] 
    private Player_Status_Controller plasta;

    [SerializeField]
    private HealthBarScript plaHealth;
    [SerializeField]
    private GameObject findHealth;

    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject findGM;

    [SerializeField]
    private StatusDate statusDate;

    [SerializeField] 
    private ExpManager expManager;

    //int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        plasta = gameObject.GetComponent<Player_Status_Controller>();
        plaHealth = findHealth.GetComponent<HealthBarScript>();
        GM = findGM.GetComponent<GameManager>();
        statusDate = GetComponent<StatusDate>();
        expManager = GetComponent<ExpManager>();

        //elementNum = statusDate.D_ExpListElement;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (plasta.PlayerExp >= expManager.ExpTablesList[statusDate.D_ExpListElement] && plasta.PlayerLevel != 99)
        {
            LevelUp();
        }

        //if (level == 99) return;
        //level++;
        //Debug.Log($"level:{level} next:{(int)needExpManage.Evaluate(level)}");
        //expManager.ExpTablesList.Add((int)needExpManage.Evaluate(level));
    }

    public void LevelUp()
    {
        plasta.PlayerLevel++;
        plasta.PlayerHP += expManager.HpGrowthTableList[statusDate.D_ExpListElement];
        plasta.PlayerAttackPower += expManager.AttackPGrowthTableList[statusDate.D_ExpListElement];
        plasta.PlayerDefance += expManager.DefanceGrowthTableList1[statusDate.D_ExpListElement];

        plasta.LivePlayerHP = plasta.PlayerHP;
        plaHealth.SetMaxHealth(plasta.PlayerHP);
        GM.Player_LevelUp_EffectAndSound(myCol,gameObject);
        
        statusDate.D_ExpListElement++;
    }
}
