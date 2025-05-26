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
    private GameObject decisionCsr;

    [SerializeField,Header("プレイヤー")]
    private GameObject findPla;

    [SerializeField,Header("遷移画面のPlayerableDirector")]
    private PlayableDirector playable;

    [SerializeField]
    private GameObject loadSceneObject; 

    private Player_Status_Controller plasta;


    private PlayerInput plain;

  
    private Flowchart flocha;

    [SerializeField,Header("FungusのFlowchart参照オブジェクト")]
    private GameObject flochaOb;

    private SoundManager soundManager;

    [SerializeField,Header("soundManagerの参照オブジェクト")]
    private GameObject findSoundManager;

    [SerializeField]
    private StatusDate statusData;

    [SerializeField]
    private FlagManagementData flagmentData;

    [SerializeField,Header("ロードするシーン名")]
    private string loadSceneName;

    [SerializeField,Header("Finngusに発行するメッセージ")]
    private string fungusSendMessage;

    bool iventing = false;

    bool decision = false;

    // Start is called before the first frame update
    void Start()
    {
        //findPla = GameObject.FindWithTag("Player");
        //flochaOb = GameObject.FindWithTag("Fungus");
  
      
        plasta = findPla.GetComponent<Player_Status_Controller>();
        plain = findPla.GetComponent<PlayerInput>();
        flocha = flochaOb.GetComponent<Flowchart>();
        soundManager = findSoundManager.GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plain.actions["Decision"].triggered)
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

            ChageAction();

            decisionCsr.SetActive(false);

            iventing = true;

        }


    }

    //public void OnDecision(InputAction.CallbackContext context)
    //{
    //      if (findPlayer == false) return;
        
    //        ChageAction();

    //        decisionCsr.SetActive(false);

    //        iventing = true;

    //        findPlayer = false;
        
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            decisionCsr.SetActive(false);

        }
    }

    //---FunGusEventで使用---//
    public void ChageAction()
    {
        plain.SwitchCurrentActionMap("UI");
        //LoadingStart();//本来ならFunGusで呼ぶ関数です。
        flocha.SendFungusMessage(fungusSendMessage);
    }

    public void OnFinishedIvent()
    {
        plain.SwitchCurrentActionMap("Player");
        iventing = false;
    }

    public void LoadingStart()
    {
        flagmentData.SceneName = loadSceneName;
        UpdateStatas();
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        playable.Play();
    }
    //------------------------//

    //public void Loading()
    //{ 
    // StartCoroutine(LoadSceneAsync("LoadingScene"));
    //}

    //IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}

    /// <summary>
    /// ステータス格納オブジェクトにそれぞれのステータスを格納するメソッド。
    /// </summary>
    private void UpdateStatas()
    { 
       statusData.D_PlayerLevel = plasta.PlayerLevel;

       statusData.D_PlayerExp = plasta.PlayerExp;

       statusData.D_PlayerHP = plasta.PlayerHP;

       statusData.D_LivePlayerHP = plasta.LivePlayerHP;

       statusData.D_PlayerAttackPower = plasta.PlayerAttackPower;

       statusData.D_PlayerDefance = plasta.PlayerDefance;
     
    }
}
