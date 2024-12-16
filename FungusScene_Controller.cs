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
  
    [SerializeField]
    private GameObject decisionCsr;

    private GameObject playerObj;

    [SerializeField]
    private PlayableDirector playable;

    [SerializeField]
    private GameObject loadSceneObject; 

    [SerializeField]
    private Player_Status_Controller psc;


    [SerializeField] 
    private PlayerInput playerInput;

    [SerializeField]
    private Flowchart flowChart;

    private GameObject flowchartObj;

    [SerializeField]
    private GameManager gameManager;

    private GameObject findGM; //同じ仕組みで複数のクラスでやるなら、同じ名前に統一しましょう

    [SerializeField]
    private StatusData statusData;

    [SerializeField]
    private FlagManagementData flagManagementData;

    [SerializeField] 
    private string loadSceneName;

    [SerializeField]
    private string fungusSendMessage;

    bool isEventActive = false;

    bool isDecisionMade = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        flowchartObj = GameObject.FindWithTag("Fungus");
        findGM = GameObject.FindWithTag("GameManager");
      
        psc = playerObj.GetComponent<Player_Status_Controller>();
        playerInput = playerObj.GetComponent<PlayerInput>();
        flowChart = flowchartObj.GetComponent<Flowchart>();
        gameManager = findGM.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Decision"].triggered)
        {
            isDecisionMade = true;

        }

        if (isDecisionMade == false) return;

        if (isDecisionMade)
        {
            Invoke("DecisionFalse", 0.1f);
        }
    }

/// <summary>
/// プレイヤーがイベントを選択したかどうかを判定するメソッド
/// </summary>
    private void DecisionFalse()
    {
        isDecisionMade = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isEventActive == false)
        {
            decisionCsr.SetActive(true);
        }


        if (other.gameObject.tag == "Player" && isEventActive == false && isDecisionMade)
        {

            ChangeAction();

            decisionCsr.SetActive(false);

            isEventActive = true;

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

/// <summary>
/// プレイヤーのアクションを変更するメソッド
/// </summary>
    public void ChangeAction()
    {
        playerInput.SwitchCurrentActionMap("UI");
        flowChart.SendFungusMessage(fungusSendMessage);
    }

/// <summary>
/// イベントが終了した際にプレイヤーのアクションを変更するメソッド
/// </summary>
    public void OnFinishedEvent()
    {
        playerInput.SwitchCurrentActionMap("Player");
        isEventActive = false;
    }

/// <summary>
/// 説明書きましょう
/// </summary>
    public void LoadingStart()
    {
       flagManagementData.SceneName = loadSceneName;
        UpdateStatus();
        gameManager.MusicManager.GetComponent<AudioSource>().Stop();
        gameManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        playable.Play();
    }

/// <summary>
///  説明書きましょう
/// </summary>
    public void Loading()
    { 
     StartCoroutine(LoadSceneAsync("LoadingScene"));
    }

/// <summary>
/// 説明書きましょう
/// </summary>
/// <param name="sceneName"></param>
/// <returns></returns>
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

/// <summary>
/// ステータスデータを更新するメソッド
/// </summary>
    private void UpdateStatus()
    { 
       statusData.D_PlayerLevel = psc.PlayerLevel;
       statusData.D_PlayerExp = psc.PlayerExp;
       statusData.D_PlayerHP = psc.PlayerHP;
       statusData.D_LivePlayerHP = psc.LivePlayerHP;
       statusData.D_PlayerAttackPower = psc.PlayerAttackPower;
       statusData.D_PlayerDefance = psc.PlayerDefance;
      
    }
}
