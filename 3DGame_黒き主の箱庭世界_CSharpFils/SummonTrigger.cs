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

    private SoundManager SoundManager;

    private GameObject findSoundManager;

    private BattleManager battleManager;

    private GameObject findBattleManager;

    private float summonIntarval;
    [SerializeField]
    private float return_SummonIntarval = 0.2f;

    private int summonCount = 0;

    [SerializeField]
    private int SummonCountLimit;

    [SerializeField]
    private int specialMonsNum;
    [SerializeField]
    private bool existSpacialMons = false;

    [SerializeField]
    private bool endLessSummon = false;


    private void Start()
    {
        //findGameManager = GameObject.FindWithTag("GameManager");

        findBattleManager = GameObject.FindWithTag("BattleManager");

        findSoundManager = GameObject.FindWithTag("SoundManager");

        //gameManager = findGameManager.GetComponent<GameManager>();

        battleManager = findBattleManager.GetComponent<BattleManager>();

        SoundManager = findSoundManager.GetComponent<SoundManager>();
    }
    private void OnTriggerStay(Collider other)
    {
        //if (!battleManager.GameStart) return;


        if (other.gameObject.tag == "Player")
        {
            
            if (summonIntarval > 0)
            {
                summonIntarval -= Time.deltaTime;
            }

            if (summonIntarval <= 0)
            { 
             Summon();
             summonIntarval = return_SummonIntarval;
            }
           

            if (summonCount >= SummonCountLimit&&endLessSummon == false)
            { 
            Destroy(gameObject);
            }
            
        }
    }

    private void Summon()
    {
        if (summonCount >= SummonCountLimit && endLessSummon == false) return;

        var num = Random.Range(0, enemyPrefabList.Count);

        var prefab = enemyPrefabList[num];

        var posNum = Random.Range(0, enemyGateList.Count);
        var pos = enemyGateList[posNum];

        var obj = Instantiate(prefab, pos.position, Quaternion.Euler(0f,180f,0f));

        battleManager.Enemy_Summon_Effect(obj);

        SoundManager.OneShot_Enemy_Action_Sound(0);

        if (existSpacialMons && num == specialMonsNum)
        {
            enemyPrefabList.RemoveAt(specialMonsNum);
        }

        if (endLessSummon) return;

        summonCount++;
    }
}
