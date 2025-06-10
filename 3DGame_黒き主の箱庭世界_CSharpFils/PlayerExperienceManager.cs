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

    
    [SerializeField]
    private Collider myCollider;

  
    private Player_Status_Controller playerStatCon;

  
    private HealthBarScript playerHealthBarScript;
    [SerializeField]
    private GameObject findHealthObj;

    private BattleManager battleManager;
    [SerializeField]
    private GameObject findBattleManager;

  

    [SerializeField]
    private StatusDate statusData;
    [SerializeField]
    private ExpManager expManager;

   
   
    void Start()
    {
        playerStatCon = gameObject.GetComponent<Player_Status_Controller>();
        playerHealthBarScript = findHealthObj.GetComponent<HealthBarScript>();
        if (findBattleManager != null)
        {
            battleManager = findBattleManager.GetComponent<BattleManager>();
        }
        
    }

   
    /// <summary>
    /// プレイヤーのレベルアップの演出と上昇したステータスをStatusDataにそれぞれ格納するメソッドです。
    /// </summary>
    public void LevelUp()
    {
        battleManager.Player_LevelUp_EffectAndSound(myCollider, gameObject);

        playerStatCon.PlayerLevel++;//レベル+1
        statusData.D_PlayerLevel = playerStatCon.PlayerLevel;//格納
        playerStatCon.PlayerHP += expManager.HpGrowthTableList[statusData.D_ExpListElement];//現段階のレベルに応じた量を上昇させる。
        statusData.D_PlayerHP = playerStatCon.PlayerHP;

        playerStatCon.PlayerAttackPower += expManager.AttackPGrowthTableList[statusData.D_ExpListElement];
        statusData.D_PlayerAttackPower = playerStatCon.PlayerAttackPower;

        playerStatCon.PlayerDefance += expManager.DefanceGrowthTableList1[statusData.D_ExpListElement];
        statusData.D_PlayerDefance = playerStatCon.PlayerDefance;

        playerStatCon.LivePlayerHP = playerStatCon.PlayerHP;//HPが満タンになる。

        playerHealthBarScript.SetMaxHealth(playerStatCon.PlayerHP);//HPスライダーの最大値にPlayerHPの値を代入。

        playerHealthBarScript.SetHealth(playerStatCon.PlayerHP);//HPスライダーにHPの値を反映。


        statusData.D_ExpListElement++;//テーブルの階層の値を+1



    }

  
}
