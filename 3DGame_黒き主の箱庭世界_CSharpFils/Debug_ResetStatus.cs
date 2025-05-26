using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// デバッグ用。プレイヤーのステータスの値を初期値に戻すクラスです。
/// </summary>
public class Debug_ResetStatus : MonoBehaviour
{
    [SerializeField]
    private StatusDate gameData;

    [SerializeField]
    private FlagManagementData flagmentData;

    [SerializeField]
    private SettingData settingData;

    [SerializeField]
    private PlayerTransformData playerTransformData;

    [SerializeField]
    bool ResetOn;

    [SerializeField]
    List<int> expTablesList = new List<int>();

    [SerializeField]
    List<int> hpGrowthTableList = new List<int>();

    [SerializeField]
    List<int> attackPGrowthTableList = new List<int>();

    [SerializeField]
    List<int> defanceGrowthTableList = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame

    /// <summary>
    /// プレイヤーのステータスを初期化。
    /// </summary>
    public void DebugReset()
    {
        if (!ResetOn) return;

        Debug.Log("ステータスを初期化しました。");
        gameData.D_PlayerLevel = 1;
        gameData.D_PlayerExp = 0;
        gameData.D_PlayerHP = 20;
        gameData.D_LivePlayerHP = 20;
        gameData.D_PlayerAttackPower = 3;
        gameData.D_PlayerDefance = 1;
        gameData.D_ExpListElement = 0;

        settingData.BgmVolume = 0.3f;
        settingData.SoundVolum = 0.5f; 
        settingData.FrameRate = 60;
        settingData.VibrationON = true;

        flagmentData.TutorialClear = false;
        flagmentData.Map1Clear = false;
        flagmentData.Map2Clear = false;
        flagmentData.GameClear = false;
        flagmentData.SceneName = "Tutorial";

        playerTransformData.LoadTransform = new Vector3(0,0,-16f);
        playerTransformData.LoadRotate = Quaternion.identity;
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          DebugReset();
        }
    }
}
