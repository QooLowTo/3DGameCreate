using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自身のコリジョンに「プレイヤー」が触れたら指定した座標に敵を召喚するクラスです。
/// </summary>
public class SummonTrigger : MonoBehaviour
{
    [SerializeField] 
    List<GameObject> enemyPrefabList = new List<GameObject>();

    [SerializeField] 
    List<Transform> enemyGateList = new List<Transform>();

    private SoundManager soundManager;

    private GameObject findSoundManager;

    private BattleManager battleManager;

    private GameObject findBattleManager;

    private float summonInterval;
    [SerializeField]
    private float return_SummonInterval = 0.2f;

    private int summonCount = 0;

    [SerializeField]
    private int summonCountLimit;

    [SerializeField]
    private int specialMonsNum; //説明書いて
    [SerializeField]
    private bool existSpecialMons = false;

    [SerializeField]
    private bool endlessSummon = false;


    private void Start()
    {

        findBattleManager = GameObject.FindWithTag("BattleManager");

        findSoundManager = GameObject.FindWithTag("SoundManager");

        battleManager = findBattleManager.GetComponent<BattleManager>();

        soundManager = findSoundManager.GetComponent<SoundManager>();
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {         
            if (summonInterval > 0)
            {
                summonInterval -= Time.deltaTime;
            }

            if (summonInterval <= 0)
            { 
             Summon();
             summonInterval = return_SummonInterval;
            }
           

            if (summonCount >= summonCountLimit&&endlessSummon == false)
            { 
            Destroy(gameObject);
            }
            
        }
    }

    private void Summon()
    {
        if (summonCount >= summonCountLimit && endlessSummon == false) return;

        var num = Random.Range(0, enemyPrefabList.Count);

        var prefab = enemyPrefabList[num];

        var posNum = Random.Range(0, enemyGateList.Count);
        var pos = enemyGateList[posNum];

        var obj = Instantiate(prefab, pos.position, Quaternion.Euler(0f,180f,0f)); //マジックナンバー発見！

        battleManager.Enemy_Summon_Effect(obj);

        soundManager.OneShot_Enemy_Action_Sound(0);

        if (existSpecialMons && num == specialMonsNum)
        {
            enemyPrefabList.RemoveAt(specialMonsNum);
        }

        if (endlessSummon) return;

        summonCount++;
    }
}
