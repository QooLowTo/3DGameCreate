using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// メインのUIの「ステータス」、「設定」などのボタンを制御するクラスです。
/// 選択されたボタンごとに演出するスライドを変えるなどの役割を持っています。
/// </summary>
public class ButtonSelector : UIManager
{
    [SerializeField]
    private string selectButtonName;

    [SerializeField]
    private List<PlayableDirector> buttonPlayables = new List<PlayableDirector>();

    [SerializeField]
    private List<GameObject> selectButton = new List<GameObject>();

    [SerializeField]
    private List<GameObject> selsectBackButton = new List<GameObject>();

    [SerializeField]
    private List<ButtonController> buttonConList = new List<ButtonController>();

    //[SerializeField]
    //private PlayableDirector savePopPlayable;

    //[SerializeField]
    //private GameObject titleBackSlide;

    //[SerializeField]
    //private GameObject backSceneSlide;

    //private UIManager uiManager;

    //private GameObject findUiManager;


    [SerializeField]
    protected FlagManagementData flagmentData;

    //ButtonController buttonController;
    //private GameObject findButtonCntr;


   

    //[SerializeField]
    //private bool centerNow;

   

   

    public List<GameObject> SelsectButton { get => selectButton; set => selectButton = value; }
    public string SelectButtonName { get => selectButtonName; set => selectButtonName = value; }
    public List<PlayableDirector> ButtonPlayables { get => buttonPlayables; set => buttonPlayables = value; }
    public List<GameObject> SelectButton { get => selectButton; set => selectButton = value; }
    public List<GameObject> SelsectBackButton { get => selsectBackButton; set => selsectBackButton = value; }

    // Start is called before the first frame update
    void Start()
    {
        
        StartUISetting();
    }

    // Update is called once per frame
    //void Update()
    //{

    //    GetSelectButtonName();

    //    //GetSelectButtonName(1);

    //    //GetSelectButtonName(2);

    //    //GetSelectButtonName(3);

    //    //GetSelectButtonName(4);

    //    //if (centerNow) return;

    //    //GetSelectButtonName(5);


    //}

    /// <summary>
    /// 選択しているボタンを取得するメソッドです。ButtonControllerで使用。
    /// </summary>
    public void GetSelectButtonName()
    {
        for (int i = 0; i < buttonConList.Count; i++)
        {
            //選択中のオブジェクトの名前がnullでないかつ
            //指定のリストの名前と一致いていたら。
            if (buttonConList[i].EventSystem.currentSelectedGameObject.name != null &&
                buttonConList[i].EventSystem.currentSelectedGameObject.name == selectButton[i].name)
            {

                selectButtonName = buttonConList[i].EventSystem.currentSelectedGameObject.name;//名前を格納する。

            }
        }

    }

    //public void SlideOpen()
    //{
       
    //    soundManager.OneShotDecisionSound();//決定サウンド

       
    //    switch (selectButtonName)
    //    {
    //        case "Status":

    //            buttonPlayables[0].Play();
               

    //            break;

    //        case "HowToOperate":

    //            buttonPlayables[1].Play();


    //            break;

    //        case "Save":
    //            buttonPlayables[2].Play();


    //            break;

    //        case "Titleback":
    //            buttonPlayables[3].Play();
              

    //            break;

    //        case "Setting":
    //            buttonPlayables[4].Play();
              

    //            break;

    //        case "SceneBack":
    //            buttonPlayables[5].Play();


    //            break;
    //    }
    //}

    //public void SlidePause()//MainUIで使用
    //{  
    //    openNow = true;

    //    cancelOK = true;

     
    //    switch (selectButtonName)
    //    {
    //        case "Status":

    //            buttonPlayables[0].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


    //            break;

    //        case "HowToOperate":

    //            buttonPlayables[1].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


    //            break;

    //        case "Save":
    //            buttonPlayables[2].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[1]);

    //            break;

    //        case "Titleback":
    //            buttonPlayables[3].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[2]);

    //            break;

    //        case "Setting":

    //            buttonPlayables[4].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


    //            break;

    //        case "SceneBack":

    //            buttonPlayables[5].Pause();

    //            EventSystem.current.SetSelectedGameObject(selsectBackButton[3]);


    //            break;
    //    }

    //}

    //public void SlideResum()//ボタン
    //{
    //    soundManager.OneShotCancelSound();//キャンセルサウンド


    //    switch (selectButtonName)
    //    {
    //        case "Status":
              
    //            buttonPlayables[0].Resume();

    //            break;

    //        case "HowToOperate":

    //            buttonPlayables[1].Resume();

    //            break;

    //        case "Save":

    //            buttonPlayables[2].Resume();

    //            break;

    //        case "Titleback":

    //            buttonPlayables[3].Resume();

    //            break;

    //        case "Setting":

    //            buttonPlayables[4].Resume();

    //            break;

    //        case "SceneBack":

    //            buttonPlayables[5].Resume();

    //            break;
    //    }
    //}

    //public void SlideClose()//MainUIで使用
    //{ 
        
    //    switch (selectButtonName)
    //    {
    //        case "Status":

    //            EventSystem.current.SetSelectedGameObject(selectButton[0]);

    //            break;

    //        case "HowToOperate":

    //            EventSystem.current.SetSelectedGameObject(selectButton[1]);

    //            break;

    //        case "Save":

    //            EventSystem.current.SetSelectedGameObject(selectButton[2]);

    //            break;

    //        case "Titleback":

    //            EventSystem.current.SetSelectedGameObject(selectButton[3]);

    //            break;

    //        case "Setting":

    //            EventSystem.current.SetSelectedGameObject(selectButton[4]);

    //            break;

    //        case "SceneBack":

    //            EventSystem.current.SetSelectedGameObject(selectButton[5]);

    //            break;
    //    }

    //    openNow = false;
    //    cancelOK=false;
    //}

    //public void SavePop()
    //{
    //    savePopPlayable.GetComponent<PlayableDirector>().Play();

    //    soundManager.OneShotDecisionSound();//決定サウンド
    //}

    //public void BackTitle()
    //{
    //    titleBackSlide.SetActive(true);

    //    flagmentData.SceneName = "Title";

    //    soundManager.OneShotDecisionSound();//決定サウンド
    //}

    //public void BackScene()
    //{
    //    backSceneSlide.SetActive(true);

    //    flagmentData.SceneName = "HomeMap";

    //    soundManager.MusicManager.GetComponent<AudioSource>().Stop();

    //    soundManager.OneShotDecisionSound();//決定サウンド

    //    soundManager.LoadingStart_Sound();
    //}

    //public void LoadScene()
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
}
