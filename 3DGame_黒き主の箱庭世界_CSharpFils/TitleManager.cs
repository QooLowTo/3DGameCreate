using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンでのイベントを管理するクラスです。
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


    [SerializeField]
    private PlayerInput playerInput; //インプットシステム

    [SerializeField]
    private GameObject titleTransition;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField] 
    private GameObject eveSys; //?? これ何用？ 名前は役割が分かるように


    [SerializeField]
    private FlagManagementData flagManagementData; //他のところと統一した方がいい

    [SerializeField]
    private PlayerTransformData playerTransformData;

    private string selectButtonName;

    private string loadSceneName;

    [SerializeField]
    private PlayableDirector saveAdviseMassage; //セーブアドバイスのタイムライン　→　合っている？

    private DateSaveUIController dateSaveUIController;

    [SerializeField,Header("dateSaveUIControllerの参照オブジェクト")]
    private GameObject findDSUCon; //説明書いて

    //デバック用
    [SerializeField,Header("デバック用(好きなシーンに移動)")]
    private bool debugLoad;
    [SerializeField]
    private List<string> debugLoadSceneNameList = new List<string>();
    [SerializeField]
    private int sceneNumber;
    
        void Start()
    {
        titleTransition.SetActive(true);

        playerInput = findPla.GetComponent<PlayerInput>();

        buttonSelector = GetComponent<ButtonSelector>();

        soundManager = findSoundManager.GetComponent<SoundManager>();


        dateSaveUIController = findDSUCon.GetComponent<DateSaveUIController>();

        titlePlayables = buttonSelector.ButtonPlayables;

        selectBackButton = buttonSelector.SelsectBackButton;

        selectButton = buttonSelector.SelsectButton;


        if (debugLoad)
        {
            switch (sceneNumber)
            {
                //数字ではなく、enumを使ってください

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
            //titlePlayablesの数字をenumなどで管理した方がいいです。
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
    /// シグナルで使用。選択されたボタンをに応じたタイムラインの再生を止めるメソッドです。
    /// </summary>
    public void BeggingPause()  //-> この名前は合っていますか？一時的止めることを
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
        flagManagementData.SceneName = loadSceneName;
          
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
