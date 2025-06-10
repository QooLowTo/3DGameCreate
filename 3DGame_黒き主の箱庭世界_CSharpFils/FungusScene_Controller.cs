using DG.Tweening;
using Fungus;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// Fungusを用いたイベントを制御するクラスです。
/// </summary>
public class FungusScene_Controller : MonoBehaviour
{
  
    [SerializeField,Header("決定カーソルオブジェクト")]
    private GameObject decisionCsr;　//書いて

    [SerializeField,Header("プレイヤー")]
    private GameObject findPlayer;

    [SerializeField,Header("遷移画面のPlayerableDirector")]
    private PlayableDirector playable;

    [SerializeField]
    private GameObject loadSceneObject; 

    private Player_Status_Controller playerStatCon;


    private PlayerInput playerInput;

  
    private Flowchart flowchart;

    [SerializeField,Header("FungusのFlowchart参照オブジェクト")]
    private GameObject flowChartObj;

    private SoundManager soundManager;

    [SerializeField,Header("soundManagerの参照オブジェクト")]
    private GameObject findSoundManager;

    [SerializeField]
    private StatusDate statusData;

    [SerializeField]
    private FlagManagementData flagManagementData;

    [SerializeField,Header("ロードするシーン名")]
    private string loadSceneName;

    [SerializeField,Header("Finngusに発行するメッセージ")]
    private string fungusSendMessage;

    bool iventing = false; //説明書いてね

    bool decision = false;

    void Start()
    {
      
        playerStatCon = findPlayer.GetComponent<Player_Status_Controller>();
        playerInput = findPlayer.GetComponent<PlayerInput>();
        flowchart = flowChartObj.GetComponent<Flowchart>();
        soundManager = findSoundManager.GetComponent<SoundManager>();
    }

    void Update()
    {
        if (playerInput.actions["Decision"].triggered)
        {
            decision = true;

        }

        if (decision == false) return;

        if (decision)
        {
            Invoke("DecisionFalse", 0.1f);
        }
    }

    private void DecisionFalse()
    {
        decision = false;
    }

    private void OnTriggerStay(Collider other)
    {
       
        if (iventing == false)
        {
            decisionCsr.SetActive(true);
        }

        if (other.gameObject.tag == "Player" && !iventing  && decision)
        {

            ChangeAction();

            decisionCsr.SetActive(false);

            iventing = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            decisionCsr.SetActive(false);

        }
    }

    //---FunGusEventで使用---//
    public void ChangeAction()
    {
        playerInput.SwitchCurrentActionMap("UI");
        //LoadingStart();//本来ならFunGusで呼ぶ関数です。
        flowchart.SendFungusMessage(fungusSendMessage);
    }

    public void OnFinishedEvent()
    {
        playerInput.SwitchCurrentActionMap("Player");
        iventing = false;
    }

    public void LoadingStart()
    {
        flagManagementData.SceneName = loadSceneName;
        UpdateStatas();
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        playable.Play();
    }
   
    /// <summary>
    /// ステータス格納オブジェクトにそれぞれのステータスを格納するメソッド。
    /// </summary>
    private void UpdateStatas()
    { 
       statusData.D_PlayerLevel = playerStatCon.PlayerLevel;

       statusData.D_PlayerExp = playerStatCon.PlayerExp;

       statusData.D_PlayerHP = playerStatCon.PlayerHP;

       statusData.D_LivePlayerHP = playerStatCon.LivePlayerHP;

       statusData.D_PlayerAttackPower = playerStatCon.PlayerAttackPower;

       statusData.D_PlayerDefance = playerStatCon.PlayerDefance;
     
    }
}
