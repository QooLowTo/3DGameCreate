using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o���l�̊Ǘ����s���X�N���v�^�u���I�u�W�F�N�g�ł��B
/// </summary>
 [CreateAssetMenu(menuName = "���x���Ǘ�/�o���l�Ǘ��I�u�W�F�N�g")]
public class ExpManager : ScriptableObject
{
    [SerializeField] 
    List<int> expTablesList = new List<int>();

    [SerializeField]
    List<int> hpGrowthTableList = new List<int>();

    [SerializeField]
    List<int> attackPGrowthTableList = new List<int>(); //����P�͉��H

    [SerializeField]
    List<int> defenceGrowthTableList = new List<int>();

    public List<int> ExpTablesList { get => expTablesList; set => expTablesList = value; }
    public List<int> HpGrowthTableList { get => hpGrowthTableList; set => hpGrowthTableList = value; }
    public List<int> AttackPGrowthTableList { get => attackPGrowthTableList; set => attackPGrowthTableList = value; }
    public List<int> DefenceGrowthTableList { get => defenceGrowthTableList; set => defenceGrowthTableList = value; }
}
