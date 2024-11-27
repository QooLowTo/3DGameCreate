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

    private GameObject PlaOb;

    [SerializeField]
    private PlayableDirector playable;

    [SerializeField]
    private GameObject loadSceneObject; 

    [SerializeField]
    private Player_Status_Controller plasta;


    [SerializeField] 
    private PlayerInput plain;

    [SerializeField]
    private Flowchart Flocha;

    private GameObject FlochaOb;

    [SerializeField]
    private GameManager GM;

    private GameObject FindGM;

    [SerializeField]
    private StatusDate statusData;

    [SerializeField]
    private FlagmentData flagmentData;

    [SerializeField] 
    private string LoadSceneName;

    [SerializeField]
    private string fungusSendMessage;

    bool iventing = false;

    bool decision = false;

    // Start is called before the first frame update
    void Start()
    {
        PlaOb = GameObject.FindWithTag("Player");
        FlochaOb = GameObject.FindWithTag("Fungus");
        FindGM = GameObject.FindWithTag("GameManager");
      
        plasta = PlaOb.GetComponent<Player_Status_Controller>();
        plain = PlaOb.GetComponent<PlayerInput>();
        Flocha = FlochaOb.GetComponent<Flowchart>();
        GM = FindGM.GetComponent<GameManager>();
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


        if (other.gameObject.tag == "Player" && iventing == false && decision)
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

    public void ChageAction()
    {
        plain.SwitchCurrentActionMap("UI");
        Flocha.SendFungusMessage(fungusSendMessage);
    }

    public void OnFinishedIvent()
    {
        plain.SwitchCurrentActionMap("Player");
        iventing = false;
    }

    public void LoadingStart()
    {
       flagmentData.SceneName = LoadSceneName;
        UpdateStatas();
        GM.MusicManager.GetComponent<AudioSource>().Stop();
        GM.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        playable.Play();
    }

    public void Loading()
    { 
     StartCoroutine(LoadSceneAsync("LoadingScene"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

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
