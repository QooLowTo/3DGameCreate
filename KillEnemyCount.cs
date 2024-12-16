using UnityEngine;

/// <summary>
/// �G��|���������J�E���g����N���X�ł��B
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

        //�ȒP�ȏ����̐�����������
        if (myEnemy.EnemyDie) return;

        if (myEnemy.EnemyHp <= 0)
        { 
           KillCountPlus();
        }
    }

/// <summary>
/// �G��|���������J�E���g���郁�\�b�h�ł��B
/// </summary>
    private void KillCountPlus()
    {
        flagManagementData.KillCount++;
      
    }
}
