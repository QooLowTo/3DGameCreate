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
    private FlagManagementData flagManagementData;

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
    List<int> attackGrowthTableList = new List<int>(); //

    [SerializeField]
    List<int> defenceGrowthTableList = new List<int>();

   
    /// <summary>
    /// プレイヤーのステータスを初期化。
    /// </summary>
    public void DebugReset()
    {
        if (!ResetOn) return;

        //
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

        flagManagementData.TutorialClear = false;
        flagManagementData.Map1Clear = false;
        flagManagementData.Map2Clear = false;
        flagManagementData.GameClear = false;
        flagManagementData.SceneName = "Tutorial";

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
