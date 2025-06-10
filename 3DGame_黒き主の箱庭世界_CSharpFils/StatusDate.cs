using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのステータスを記録するクラスです。
/// </summary>
[CreateAssetMenu(menuName = "スクリプタブ/ステータス管理オブジェクト")]
public class StatusData : ScriptableObject
{
    //デフォルトデータ
    [SerializeField] 
    private int d_PlayerLevel = 1;

    [SerializeField]
    private string d_playerName;

    [SerializeField] 
    private int d_PlayerHP = 20;
    [SerializeField]
    private int d_LivePlayerHP = 20;
    [SerializeField] 
    private int d_PlayerAttackPower = 3;
    [SerializeField]
    private int d_PlayerDefance = 1;
    [SerializeField]
    private int d_PlayerExp = 0;
    [SerializeField]
    private int d_ExpListElement = 0;



    public int D_PlayerLevel { get => d_PlayerLevel; set => d_PlayerLevel = value; } 
    public string D_playerName { get => d_playerName; set => d_playerName = value; }
    public int D_PlayerHP { get => d_PlayerHP; set => d_PlayerHP = value; }
    public int D_LivePlayerHP { get => d_LivePlayerHP; set => d_LivePlayerHP = value; }
    public int D_PlayerAttackPower { get => d_PlayerAttackPower; set => d_PlayerAttackPower = value; }
    public int D_PlayerDefance { get => d_PlayerDefance; set => d_PlayerDefance = value; }
    public int D_PlayerExp { get => d_PlayerExp; set => d_PlayerExp = value; }
    public int D_ExpListElement { get => d_ExpListElement; set => d_ExpListElement = value; }
}
