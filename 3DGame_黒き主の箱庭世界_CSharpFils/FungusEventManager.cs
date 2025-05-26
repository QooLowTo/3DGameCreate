using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// チュートリアル中の案内の演出を制御するクラスです。
/// </summary>
public class FungusEventManager : GameManager
{
    private Player_Tutorial_Controller plaTCon;

    private SoundManager soundManager;

    private BattleManager battleManager;

    [SerializeField]
    private GameObject findSoundManager;

    [SerializeField]
    private GameObject findBattleManager;

    private Flowchart tutorialFlow;
  
    //private GameObject findTutorialFlow;

    [SerializeField]
    private FlagManagementData flagManagementDate;

    [SerializeField]
    private List<PlayableDirector> tutorialPlayable = new List<PlayableDirector>();

    [SerializeField]
    private List<GameObject> tutorialObject = new List<GameObject>();

    [SerializeField]
    private List<Collider> changeTutorialColList = new List<Collider>();

    private bool cameraTutorialing = false;

    private bool moveTutorialing = false;

    private bool jumpTutorialing = false;

    private bool dashTutorialing = false;

    private bool attackTutorialing = false;

    private bool avoidanceTutorial = false;

    public bool CameraTutorialing { get => cameraTutorialing; set => cameraTutorialing = value; }
    public bool MoveTutorialing { get => moveTutorialing; set => moveTutorialing = value; }
    public bool JumpTutorialing { get => jumpTutorialing; set => jumpTutorialing = value; }
    public bool DashTutorialing { get => dashTutorialing; set => dashTutorialing = value; }
    public bool AttackTutorialing { get => attackTutorialing; set => attackTutorialing = value; }
    public bool AvoidanceTutorial { get => avoidanceTutorial; set => avoidanceTutorial = value; }
    public List<GameObject> TutorialObject { get => tutorialObject; set => tutorialObject = value; }
    public List<Collider> ChangeTutorialColList { get => changeTutorialColList; set => changeTutorialColList = value; }
    public Flowchart TutorialFlow { get => tutorialFlow; set => tutorialFlow = value; }
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundManager = findSoundManager.GetComponent<SoundManager>();

        battleManager = findBattleManager.GetComponent<BattleManager>();

        //adios = gameObject.GetComponent<AudioSource>();

        //gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;

        tutorialFlow = gameObject.GetComponent<Flowchart>();

        plaTCon = findPla.GetComponent<Player_Tutorial_Controller>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (flagManagementDate.KillCount)
        {
            case 2:

                if (attackTutorialing == false) return;
                tutorialFlow.SendFungusMessage("AvoidanceTutorial");
                attackTutorialing = false;

                break;

            case 5:

                if (avoidanceTutorial == false) return;
                tutorialFlow.SendFungusMessage("LevelUpTutorial");
                avoidanceTutorial = false;

                break;

            case 10:

                tutorialFlow.SendFungusMessage("TutorialEnd");
                Time.timeScale /= 2.0f;
                StartCoroutine(TimeReturn());
                flagManagementDate.KillCount = 0;

                break;

                IEnumerator TimeReturn()
                {
                    yield return new WaitForSeconds(5f);
                    Time.timeScale = 1.0f;
                    tutorialObject[4].SetActive(false);
                    tutorialObject[5].SetActive(false);
                    tutorialObject[6].SetActive(false);
                }
        }
    }

    public void OnTutorialInputOn()
    {
        plaTCon.PlaIn.enabled = true;
    }

    public void OntutorialStart()
    {
        plaTCon.CharaCon.enabled = true;

        plaTCon.GameStart = true;
    }


    //public void Tutorial_Decision()
    //{
    //    soundManager.OneShotDecisionSound();
    //}

    //public void Tutorial_CancelSound()
    //{
    //    soundManager.OneShotDecisionSound();
    //}

    public void ChageActionUI()
    {
        plaTCon.PlaIn.SwitchCurrentActionMap("UI");
    }

    public void ChageActionPlayer()
    {
        plaTCon.PlaIn.SwitchCurrentActionMap("Player");
    }

    public void CameraTutorialOpen()
    {
        tutorialObject[0].SetActive(true);
        tutorialPlayable[0].Play();
        tutorialFlow.SendFungusMessage("CameraTutorial");
        cameraTutorialing = true;
    }

    public void MoveTutorialOpen()
    {
        tutorialObject[0].SetActive(false);
        tutorialObject[1].SetActive(true);
        tutorialPlayable[1].Play();
        tutorialFlow.SendFungusMessage("MoveTutorial");
        moveTutorialing = true;
    }

    public void JumpTutorialOpen()
    {
        tutorialObject[2].SetActive(true);
        tutorialPlayable[2].Play();
        jumpTutorialing = true;
    }

    public void DashTutorialOpen()
    {
        tutorialObject[3].SetActive(true);
        tutorialPlayable[3].Play();
        dashTutorialing = true;
    }

    public void AttackTutorialOpen()
    {
        tutorialObject[4].SetActive(true);
        tutorialPlayable[4].Play();
        attackTutorialing = true;
    }

    public void AvoidanceTutorialOpen()
    {
        tutorialObject[5].SetActive(true);
        tutorialPlayable[5].Play();
        avoidanceTutorial = true;
    }

    public void SendMessageLockOn()
    {
        tutorialFlow.SendFungusMessage("LockOnTutorial");
    }

    public void LockOnTutorialOpen()
    {
        tutorialObject[6].SetActive(true);
        tutorialPlayable[6].Play();
    }

    
    public void Tutorial_Decision()
    {
      soundManager.OneShotDecisionSound();
    }

    public void Tutorial_CancelSound()
    {
     soundManager.OneShotDecisionSound();
    }
    public void Get_Weapon_Sound()
    {
        soundManager.OneShot_Other_Sound(0);
    }

    public void Portal_Appearance_Sound()
    {
        soundManager.OneShot_Other_Sound(1);
    }
}
