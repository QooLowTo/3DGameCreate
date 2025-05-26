using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// UIで用いる様々なクラスの親クラスです。
/// </summary>
public class UIManager : MonoBehaviour
{
    protected Player_Battle_Controller plaBCon;

    protected Player_UI_Controller plaUCon;

    protected Player_Status_Controller plaSCon;

    protected Player_Experience_Manager plaExpManeger;

    protected BossGolem bossGolem;

    protected ButtonSelector buttonOpen;

    protected SettingBarController settingBar;

    protected SoundManager soundManager;

    protected GameOverManager gameOverManager;

    protected SaveLoadSystem saveLoadSystem;

    protected DateSaveUIController dataSaveUIController;

    protected GameObject findPlaCon;

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
    private GameObject findGOM;

    [SerializeField]
    private GameObject findDataSave;

   

   
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }
    public SaveLoadSystem SaveLoadSystem { get => saveLoadSystem; set => saveLoadSystem = value; }
    public Player_Battle_Controller PlaBCon { get => plaBCon; set => plaBCon = value; }
    public Player_UI_Controller PlaUCon { get => plaUCon; set => plaUCon = value; }
    public Player_Status_Controller PlaSCon { get => plaSCon; set => plaSCon = value; }
    public Player_Experience_Manager PlaExpManeger { get => plaExpManeger; set => plaExpManeger = value; }
    public ButtonSelector ButtonOpen { get => buttonOpen; set => buttonOpen = value; }
    public SettingBarController SettingBar { get => settingBar; set => settingBar = value; }

    public GameOverManager GameOverManager { get => gameOverManager; set => gameOverManager = value; }

    public DateSaveUIController DataSaveUIController { get => dataSaveUIController; set => dataSaveUIController = value; }
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUISetting();
    }


    protected void StartUISetting()
    {
        findPlaCon = GameObject.FindWithTag("Player");

        findGolem = GameObject.FindWithTag("Enemy");

        //findSoundManager = GameObject.FindWithTag("GameManager");

        findSaveLoad = GameObject.FindWithTag("SaveAndLoad");

        if (findPlaCon != null)
        {

            plaBCon = findPlaCon.GetComponent<Player_Battle_Controller>();

            plaUCon = findPlaCon.GetComponent<Player_UI_Controller>();

            plaSCon = findPlaCon.GetComponent<Player_Status_Controller>();

            plaExpManeger = findPlaCon.GetComponent<Player_Experience_Manager>();
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

    //public void SetSelectButtonName(List<ButtonController> buttonConList,List<GameObject> selectButton,string selectButtonName)
    //{
    //    for (int i = 0; i < buttonConList.Count; i++)
    //    {
    //        //選択中のオブジェクトの名前がnullでないかつ
    //        //指定のリストの名前と一致いていたら。
    //        if (buttonConList[i].EventSystem.currentSelectedGameObject.name != null &&
    //            buttonConList[i].EventSystem.currentSelectedGameObject.name == selectButton[i].name)
    //        {

    //            selectButtonName = buttonConList[i].EventSystem.currentSelectedGameObject.name;//名前を格納する。

    //        }
    //    }


    //}
    // Update is called once per frame
    //void Update()
    //{

    //}

    //public void SiganalChangeActionMap()
    //{ 
    //plaUCon.ChangeActionMap();
    //}

    //public void SignalGameStart()
    //{
    //    soundManager.OnGameStart();
    //}

    ////public void SiganalUI_Slide_Sound()
    ////{
    ////    soundManager.Adios.PlayOneShot(soundManager.Ui_Sounds[1]);
    ////}

    //public void SingnalBossActionStart()
    //{
    //    bossGolem.BossActionStart = false;

    //    bossGolem.FindBossHPSlide.SetActive(true);
    //}

    ////public void SignalMenuClose()
    ////{ 
    ////plaUCon.MenuClose();
    ////}
    //public void SiganalSlidePause()
    //{ 
    //buttonOpen.SlidePause();
    //}

    //public void SingnalSlideClose()
    //{ 
    //buttonOpen.SlideClose();
    //}

    //public void SingnalLoading()
    //{
    //    StartCoroutine(LoadSceneAsync("LoadingScene"));

    //    IEnumerator LoadSceneAsync(string sceneName)
    //    {
    //        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    //        while (!asyncLoad.isDone)
    //        {
    //            yield return null;
    //        }
    //    }
    //}

    //public void SiganalSaveSettingValue()
    //{ 
    //settingBar.SaveSettingValue();
    //}

    ////public void SiganalGameOverPlayableStop()
    ////{ 
    ////gameOverManager.GameOverPlayableStop();
    ////}

    //public void SiganalAdvisPause()
    //{ 
    //dataSaveUIController.AdvisPause();
    //}

    //public void SinganalAdvisStop()
    //{ 
    //dataSaveUIController.AdvisStop();
    //}


}
