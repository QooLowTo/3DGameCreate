using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
/// <summary>
/// タイトルシーンでのイベントを制御するクラスです。
/// </summary>
public class TitleManager : GameManager
{
    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

    private ButtonSelector buttonSelector;

    private List<PlayableDirector> titlePlayables = new List<PlayableDirector>();

  
    private List<GameObject> selectButton = new List<GameObject>();


    private List<GameObject> selectBackButton = new List<GameObject>();

   
    //private List<ButtonController> buttonConList = new List<ButtonController>();

    [SerializeField]
    private PlayerInput plain;//インプットシステム
    //[SerializeField]
    //private GameObject findpla;

    [SerializeField]
    private GameObject titleTransition;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField] 
    private GameObject eveSys;

    //private SoundManager soundManager;

    //[SerializeField]
    //private GameObject findSoundManager;

    [SerializeField]
    private FlagManagementData gameData;

    [SerializeField]
    private PlayerTransformData playerTransformData;

    private string selectButtonName;

    private string loadSceneName;

    [SerializeField]
    private PlayableDirector saveAdvisMassage;

    private DateSaveUIController dateSaveUIController;

    [SerializeField,Header("dateSaveUIControllerの参照オブジェクト")]
    private GameObject findDSUCon;

    //デバック用
    [SerializeField,Header("デバック用(好きなシーンに移動)")]
    private bool debugLoad;
    [SerializeField]
    private List<string> debugLoadSceneNameList = new List<string>();
    [SerializeField]
    private int sceneNumber;
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleTransition.SetActive(true);

        plain = findPla.GetComponent<PlayerInput>();

        buttonSelector = GetComponent<ButtonSelector>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        //gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;
        //soundManager = findSoundManager.GetComponent<SoundManager>();

        dateSaveUIController = findDSUCon.GetComponent<DateSaveUIController>();

        titlePlayables = buttonSelector.ButtonPlayables;

        selectBackButton = buttonSelector.SelsectBackButton;

        selectButton = buttonSelector.SelsectButton;


        if (debugLoad)
        {
            switch (sceneNumber)
            {

                case 0:
                    loadSceneName = debugLoadSceneNameList[0];
                    break;

                case 1:
                    loadSceneName = debugLoadSceneNameList[1];
                    break;

                case 2:
                    loadSceneName = debugLoadSceneNameList[2];
                    break;

                case 3:
                    loadSceneName = debugLoadSceneNameList[3];
                    break;

                case 4:
                    loadSceneName = debugLoadSceneNameList[4];
                    break;

            }
        }
        else
        {
            loadSceneName = "Tutorial";
        }

    }

    //void Update()
    //{
    //    GetSelectButtonName(0);

    //    GetSelectButtonName(1);

    //    GetSelectButtonName(2);

    //    GetSelectButtonName(3);

    //}

    //private void GetSelectButtonName(int num)
    //{
    //    if (buttonConList[num].EventSystem.currentSelectedGameObject.name != null && buttonConList[num].EventSystem.currentSelectedGameObject.name == selectButton[num].name)
    //    {
    //        selectButtonName = buttonConList[num].EventSystem.currentSelectedGameObject.name;
    //    }

    //}

 　/// <summary>
   ///シグナルで使用。タイトル演出の制御用。 
   /// </summary>
    public void TitleStart()
    {
       eveSys.SetActive(true);
    }

    /// <summary>
    /// ボタンで使用。選択されたボタンをに応じてメニューの遷移を行うメソッドです。
    /// </summary>
    public void Begging()
    {
        selectButtonName = buttonSelector.SelectButtonName;

        soundManager.OneShotDecisionSound();//決定サウンド

        switch (selectButtonName)
        {
            case "最初からボタン":

                titlePlayables[0].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[0]);

                break;

            case "続きからボタン":
                titlePlayables[1].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[1]);

                break;

            case "終了ボタン":
                titlePlayables[2].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[2]);

                break;

            case "クレジット表示ボタン":
                titlePlayables[3].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[3]);
                break;
        }
      
    }
    /// <summary>
    /// シグナルで使用。選択されたボタンをに応じたタイムラインの再生を止めるメソッドです。。
    /// </summary>
    public void BeggingPause()
    {

      

        switch (selectButtonName)
        {
            case "最初からボタン":

                titlePlayables[0].Pause();


                break;

            case "続きからボタン":

                titlePlayables[1].Pause();


                break;

            case "終了ボタン":
                titlePlayables[2].Pause();

                break;

            case "クレジット表示ボタン":
                titlePlayables[3].Pause();

                break;
        }
    }
    /// <summary>
    /// ボタンで使用。メニューのキャンセルするメソッドです。
    /// </summary>
    public void BeggingResum()
    {


        soundManager.OneShotCancelSound();//キャンセルサウンド

        switch (selectButtonName)
        {
            case "最初からボタン":

                titlePlayables[0].Resume();


                break;

            case "続きからボタン":

                titlePlayables[1].Resume();


                break;

            case "終了ボタン":
                titlePlayables[2].Resume();

                break;
            case "クレジット表示ボタン":
                titlePlayables[3].Resume();

                break;
        }
    }
    /// <summary>
    /// シグナルで使用。キャンセル後、それぞれ選択していたボタンにカーソルを戻すメソッドです。
    /// </summary>
    public void ReturnButton()
    {
        switch (selectButtonName)
        {
            case "最初からボタン":

                EventSystem.current.SetSelectedGameObject(selectButton[0]);

                break;

            case "続きからボタン":

                EventSystem.current.SetSelectedGameObject(selectButton[1]);

                break;

            case "終了ボタン":

                EventSystem.current.SetSelectedGameObject(selectButton[2]);

                break;

            case "クレジット表示ボタン":

                EventSystem.current.SetSelectedGameObject(selectButton[2]);

                break;
        }

    }

    public void TitleLoadAdvisPause()
    {
      dateSaveUIController.AdvisPause();
    }

    public void TitleLoadAdvisStop() 
    {
    dateSaveUIController.AdvisStop();
    }

    /// <summary>
    /// ボタンで使用。初めからプレイさせるメソッド。
    /// </summary>
    public void PlayStrart()
    {
        gameData.SceneName = loadSceneName;
          
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[4].Play();
    }
    /// <summary>
    /// ボタンで使用。続きからプレイさせるメソッド。
    /// </summary>
    public void SuccesionStart()
    {
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[4].Play();
    }

    
    //public void Loding()
    //{
    //    StartCoroutine(LoadSceneAsync("LoadingScene"));
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
    /// ゲームを終了させるメソッド。
    /// </summary>
    public void GameQuit()
    {
        soundManager.OneShotDecisionSound();//決定サウンド
        //Invoke("Quit",1f);
        StartCoroutine(Quit());
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
