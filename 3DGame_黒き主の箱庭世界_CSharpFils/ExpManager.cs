using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 必要経験値量や各種ステータスの上昇量をそれぞれ格納したテーブルを管理するクラスです。 
/// </summary>
 [CreateAssetMenu(menuName = "スクリプタブ/経験値管理オブジェクト")]
public class ExpManager : ScriptableObject
{
    [SerializeField] 
    List<int> expTablesList = new List<int>(); //説明書いて

    [SerializeField]
    List<int> hpGrowthTableList = new List<int>(); //説明書いて

    [SerializeField]
    List<int> attackPGrowthTableList = new List<int>(); //説明書いて

    [SerializeField]
    List<int> DefenceGrowthTableList = new List<int>(); //説明書いて

    //以下プロパティ
    
    public List<int> ExpTablesList { get => expTablesList; set => expTablesList = value; }
    public List<int> HpGrowthTableList { get => hpGrowthTableList; set => hpGrowthTableList = value; }
    public List<int> AttackPGrowthTableList { get => attackPGrowthTableList; set => attackPGrowthTableList = value; }
    public List<int> DefanceGrowthTableList1 { get => DefenceGrowthTableList; set => DefenceGrowthTableList = value; }
}
