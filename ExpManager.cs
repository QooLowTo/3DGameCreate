using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経験値の管理を行うスクリプタブルオブジェクトです。
/// </summary>
 [CreateAssetMenu(menuName = "レベル管理/経験値管理オブジェクト")]
public class ExpManager : ScriptableObject
{
    [SerializeField] 
    List<int> expTablesList = new List<int>();

    [SerializeField]
    List<int> hpGrowthTableList = new List<int>();

    [SerializeField]
    List<int> attackPGrowthTableList = new List<int>(); //このPは何？

    [SerializeField]
    List<int> defenceGrowthTableList = new List<int>();

    public List<int> ExpTablesList { get => expTablesList; set => expTablesList = value; }
    public List<int> HpGrowthTableList { get => hpGrowthTableList; set => hpGrowthTableList = value; }
    public List<int> AttackPGrowthTableList { get => attackPGrowthTableList; set => attackPGrowthTableList = value; }
    public List<int> DefenceGrowthTableList { get => defenceGrowthTableList; set => defenceGrowthTableList = value; }
}
