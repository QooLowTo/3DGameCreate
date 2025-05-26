using Fungus;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
/// <summary>
/// チュートリアルで起こる様々なイベントを管理するクラスです。
/// </summary>
public class TutorialIvent : MonoBehaviour
{
    [SerializeField]
    private GameObject decisionCsr;

    [SerializeField]
    private GameObject playerWeapon;

    [SerializeField]
    private GameObject tutorialEnemy;

    [SerializeField] 
    private GameObject escapePortal;

    [SerializeField]
    private NavMeshSurface nav;
    [SerializeField]
    private GameObject navObj;


    private PlayerInput plain;
    [SerializeField]
    private GameObject findPla;


    private Flowchart flowChart;
    [SerializeField]
    private GameObject findFlow;

    [SerializeField]
    private GameObject musicManager;

    //private GameObject findGameManager;

    private SoundManager SoundManager;

    private BattleManager battleManager;

    [SerializeField]
    private GameObject findBattleManager;

    [SerializeField]
    private GameObject findSoundManager;

    [SerializeField]
    private List<Transform> enemyGateList = new List<Transform>();

    int gateNum = 0;

    bool iventing = false;

    bool decision = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //findPla = GameObject.FindWithTag("Player");
        //findFlow = GameObject.FindWithTag("Fungus");
        //findGameManager = GameObject.FindWithTag("GameManager");
        

        nav = navObj.GetComponent<NavMeshSurface>();
        plain = findPla.GetComponent<PlayerInput>();
        flowChart = findFlow.GetComponent<Flowchart>();

        battleManager = findBattleManager.GetComponent<BattleManager>();

        SoundManager = findSoundManager.GetComponent<SoundManager>();
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
            Invoke("DecisionFalse",0.1f);
        }
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

    private void DecisionFalse()
    {
        decision = false;
    }

    //public void OnDicision(InputAction.CallbackContext context)
    //{
    //    if (findPlayer == false) return;

    //    ChageAction();

    //    decisionCsr.SetActive(false);

    //    iventing = true;

    //    findPlayer = false;

    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            decisionCsr.SetActive(false);

        }
    }

    private void ChageAction()
    {
        plain.SwitchCurrentActionMap("UI");
        flowChart.SendFungusMessage("IventON");
    }

    public void OnFinishedIvent()
    {
        plain.SwitchCurrentActionMap("Player");
        iventing = false;
    }

    public void GetWeapon()
    {
        Destroy(playerWeapon,0.1f);
    }

    public void TutorialBattleStart()
    {
        flowChart.SendFungusMessage("AttackTutorial");
    }

    public void MusicStart()
    { 
       musicManager.GetComponent<AudioSource>().Play();
    }
    public void SummonTutorialEnemy()
    {
        var obj = Instantiate(tutorialEnemy, enemyGateList[gateNum].position, Quaternion.Euler(0f, 180f, 0f));

        battleManager.Enemy_Summon_Effect(obj);

        SoundManager.OneShot_Enemy_Action_Sound(0);

        gateNum++;

        if (gateNum == 3)
        { 
        gateNum = 0;
        }
    }

    public void NavStart()
    {
       nav.enabled = true;
    }

    public void PortalOn()
    { 
    escapePortal.SetActive(true);
    }
}
