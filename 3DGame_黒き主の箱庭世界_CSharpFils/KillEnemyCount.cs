using UnityEngine;

/// <summary>
/// 敵を倒した数をカウントさせるクラスです。
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
        if (myEnemy.EnemyDie) return;

        if (myEnemy.EnemyHp <= 0)
        { 
           KillCountPlus();
        }
    }

    private void KillCountPlus()
    {
        flagManagementData.KillCount++;
      
    }
}
