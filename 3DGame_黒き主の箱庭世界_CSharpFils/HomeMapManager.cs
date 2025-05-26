﻿using UnityEngine;
/// <summary>
/// バトル外でのゲームの進行等を管理するクラスです。
/// </summary>
public class HomeMapManager : GameManager
{
    private Player_Rest_Controller plaRes;

    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartGameSetting();

        StartGetPlayerStatus();

        plaRes = findPla.GetComponent<Player_Rest_Controller>();

       

        //StartGetPlayerStatus(findPla);

        if (startUI != null && !debugMode)
        {

            startUI.SetActive(true);

            soundManager.OneShot_UI_Sound(7);//ロード完了音
        }

        if (debugMode)
        {

            StartCoroutine(DebugGameStart());

        }
    }

    // Update is called once per frame
   
}
