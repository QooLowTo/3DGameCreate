using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブスロットごとにデータを分ける際に用いるクラスです。
/// </summary>

[CreateAssetMenu(menuName = "スクリプタブ/セーブスロットデータ記録オブジェクト")]
public class MemorizeLevelAndSceneName : ScriptableObject
{
    [SerializeField]
    private List<string> savedPlayerLevelList = new List<string>(); //説明書いて

    [SerializeField]
    private List<string> savedSceneNameList = new List<string>();

    public List<string> SavedPlayerLevelList { get => savedPlayerLevelList; set => savedPlayerLevelList = value; }
    public List<string> SavedSceneNameList { get => savedSceneNameList; set => savedSceneNameList = value; }
}
