using UnityEngine;

/// <summary>
/// 敵を倒した数をカウントするクラスです。
/// </summary>
public class KillEnemyCount : MonoBehaviour
{
   
    private Enemy myEnemy;

    [SerializeField]
    private FlagManagementData flagManagementData;

      void Start()
    {
        myEnemy = GetComponent<Enemy>();
    }

    void Update()
    { 

        //簡単な処理の説明を書いて
        if (myEnemy.EnemyDie) return;

        if (myEnemy.EnemyHp <= 0)
        { 
           KillCountPlus();
        }
    }

/// <summary>
/// 敵を倒した数をカウントするメソッドです。
/// </summary>
    private void KillCountPlus()
    {
        flagManagementData.KillCount++;
      
    }
}
