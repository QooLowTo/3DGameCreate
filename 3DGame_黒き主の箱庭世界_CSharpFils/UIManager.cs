using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UIで用いる様々なクラスの親クラスです。
/// </summary>
public class UIManager : MonoBehaviour
{
    protected Player_Battle_Controller playerBattleCon;

    protected Player_UI_Controller playerUICon;

    protected Player_Status_Controller playerStatCon;

    protected Player_Experience_Manager playerEXPManager;

    protected BossGolem bossGolem;

    protected ButtonSelector buttonOpen;

    protected SettingBarController settingBar;

    protected SoundManager soundManager;

    protected GameOverManager gameOverManager;

    protected SaveLoadSystem saveLoadSystem;

    protected DateSaveUIController dataSaveUIController;

    protected GameObject findPlayerCon;

    protected GameObject findGolem;

    [SerializeField]
    protected GameObject findSoundManager;

    protected GameObject findSaveLoad;

    [SerializeField]
    protected StatusDate statusDate;
    [SerializeField]
    protected ExpManager expManager;
    [SerializeField]
    protected SettingData settingData;

    [SerializeField]
    private GameObject findButtonSelector;

    [SerializeField]
    private GameObject findSettingBar;

    [SerializeField]
    private GameObject findGOM; //名前が分かりづらいからもっと役割に合った名前に変更してください。

    [SerializeField]
    private GameObject findDataSave;

   

   
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }
    public SaveLoadSystem SaveLoadSystem { get => saveLoadSystem; set => saveLoadSystem = value; }
    public Player_Battle_Controller PlayerBattleCon { get => playerBattleCon; set => playerBattleCon = value; }
    public Player_UI_Controller PlayerUICon { get => playerUICon; set => playerUICon = value; }
    public Player_Status_Controller PlayerStatCon { get => playerStatCon; set => playerStatCon = value; }
    public Player_Experience_Manager PlayerEXPManager { get => playerEXPManager; set => playerEXPManager = value; }
    public ButtonSelector ButtonOpen { get => buttonOpen; set => buttonOpen = value; }
    public SettingBarController SettingBar { get => settingBar; set => settingBar = value; }

    public GameOverManager GameOverManager { get => gameOverManager; set => gameOverManager = value; }

    public DateSaveUIController DataSaveUIController { get => dataSaveUIController; set => dataSaveUIController = value; }
   

    void Start()
    {
        StartUISetting();
    }


    protected void StartUISetting()
    {
        findPlayerCon = GameObject.FindWithTag("Player");

        findGolem = GameObject.FindWithTag("Enemy");

        findSaveLoad = GameObject.FindWithTag("SaveAndLoad");

        if (findPlayerCon != null)
        {

            playerBattleCon = findPlayerCon.GetComponent<Player_Battle_Controller>();

            playerUICon = findPlayerCon.GetComponent<Player_UI_Controller>();

            playerStatCon = findPlayerCon.GetComponent<Player_Status_Controller>();

            playerEXPManager = findPlayerCon.GetComponent<Player_Experience_Manager>();
        }

        if (findGolem != null)
        {

            bossGolem = findGolem.GetComponent<BossGolem>();
        }

        if (findSoundManager != null)
        {

            soundManager = findSoundManager.GetComponent<SoundManager>();

        }

        if (findSaveLoad != null)
        {

            saveLoadSystem = findSaveLoad.GetComponent<SaveLoadSystem>();

        }

        if (findButtonSelector != null) { 

         buttonOpen = findButtonSelector.GetComponent<ButtonSelector>();

        }


        if (findGOM != null) { 

         gameOverManager = findGOM.GetComponent<GameOverManager>();

        }

        if (findSettingBar != null) { 
        
          settingBar = findSettingBar.GetComponent<SettingBarController>();

        }

        if (findDataSave != null) { 
            dataSaveUIController = findDataSave.GetComponent<DateSaveUIController>();
        }
    
    }

}
