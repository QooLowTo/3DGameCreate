using UnityEngine;

public class KillEnemyCount : MonoBehaviour
{
   
    private Enemy myEnemy;

    [SerializeField]
    private FlagManagementData gameDate;

  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myEnemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
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
        gameDate.KillCount++;
      
    }
}
