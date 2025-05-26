using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのレベルアップを管理するクラスです。
/// </summary>
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
    /// <summary>
    /// プレイヤーのレベルアップの演出と上昇したステータスをStatusDataにそれぞれ格納するメソッドです。
    /// </summary>
    public void LevelUp()
    {
        battleManager.Player_LevelUp_EffectAndSound(myCol, gameObject);

        plasta.PlayerLevel++;//レベル+1
        statusDate.D_PlayerLevel = plasta.PlayerLevel;//格納
        //D_ExpListElementはテーブルの階層の値。(レベル2に上がったのなら、階層の値が0の値が適応される)
        plasta.PlayerHP += expManager.HpGrowthTableList[statusDate.D_ExpListElement];//現段階のレベルに応じた量を上昇させる。
        statusDate.D_PlayerHP = plasta.PlayerHP;

        plasta.PlayerAttackPower += expManager.AttackPGrowthTableList[statusDate.D_ExpListElement];
        statusDate.D_PlayerAttackPower = plasta.PlayerAttackPower;

        plasta.PlayerDefance += expManager.DefanceGrowthTableList1[statusDate.D_ExpListElement];
        statusDate.D_PlayerDefance = plasta.PlayerDefance;

        plasta.LivePlayerHP = plasta.PlayerHP;//HPが満タンになる。

        plaHealth.SetMaxHealth(plasta.PlayerHP);//HPスライダーの最大値にPlayerHPの値を代入。

        plaHealth.SetHealth(plasta.PlayerHP);//HPスライダーにHPの値を反映。


        statusDate.D_ExpListElement++;//テーブルの階層の値を+1



    }

  
}
