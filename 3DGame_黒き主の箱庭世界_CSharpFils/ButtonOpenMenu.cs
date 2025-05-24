using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ButtonOpenMenu : MonoBehaviour
{
    private ButtonSelector buttonSelector;

    private SoundManager soundManager;

    [SerializeField]
    private GameObject findSoundManager;

    string selectButtonName;

    private bool openNow = false;

    private bool cancelOK = false;//UIコントローラーで使う

    [SerializeField]
    private PlayableDirector savePopPlayable;

    [SerializeField]
    private GameObject titleBackSlide;

    [SerializeField]
    private GameObject backSceneSlide;

    [SerializeField]
    private FlagManagementData flagmentData;

    private List<PlayableDirector> buttonPlayables = new List<PlayableDirector>();

    private List<GameObject> selsectBackButton  = new List<GameObject>();

    private List<GameObject> selectButton = new List<GameObject>();
    public bool OpenNow { get => openNow; set => openNow = value; }
    public bool CancelOK { get => cancelOK; set => cancelOK = value; }
    public List<GameObject> SelectButton { get => selectButton; set => selectButton = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonSelector = GetComponent<ButtonSelector>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        buttonPlayables = buttonSelector.ButtonPlayables;

        selsectBackButton = buttonSelector.SelsectBackButton;

        selectButton = buttonSelector.SelectButton;
    }

    // Update is called once per frame
    public void SlideOpen()
    {

        soundManager.OneShotDecisionSound();//決定サウンド

        selectButtonName = buttonSelector.SelectButtonName;


        switch (selectButtonName)
        {
            case "Status":

                buttonPlayables[0].Play();


                break;

            case "HowToOperate":

                buttonPlayables[1].Play();


                break;

            case "Save":
                buttonPlayables[2].Play();


                break;

            case "Titleback":
                buttonPlayables[3].Play();


                break;

            case "Setting":
                buttonPlayables[4].Play();


                break;

            case "SceneBack":
                buttonPlayables[5].Play();


                break;
        }
    }

    public void SlidePause()//SignalManagerで使用
    {
        openNow = true;

        cancelOK = true;

        selectButtonName = buttonSelector.SelectButtonName;

        switch (selectButtonName)
        {
            case "Status":

                buttonPlayables[0].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


                break;

            case "HowToOperate":

                buttonPlayables[1].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


                break;

            case "Save":
                buttonPlayables[2].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[1]);

                break;

            case "Titleback":
                buttonPlayables[3].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[2]);

                break;

            case "Setting":

                buttonPlayables[4].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[0]);


                break;

            case "SceneBack":

                buttonPlayables[5].Pause();

                EventSystem.current.SetSelectedGameObject(selsectBackButton[3]);


                break;
        }

    }

    public void SlideResum()//ボタン
    {
        soundManager.OneShotCancelSound();//キャンセルサウンド

        selectButtonName = buttonSelector.SelectButtonName;

        switch (selectButtonName)
        {
            case "Status":

                buttonPlayables[0].Resume();

                break;

            case "HowToOperate":

                buttonPlayables[1].Resume();

                break;

            case "Save":

                buttonPlayables[2].Resume();

                break;

            case "Titleback":

                buttonPlayables[3].Resume();

                break;

            case "Setting":

                buttonPlayables[4].Resume();

                break;

            case "SceneBack":

                buttonPlayables[5].Resume();

                break;
        }
    }

    public void SlideClose()//MainUIで使用
    {

        switch (selectButtonName)
        {
            case "Status":

                EventSystem.current.SetSelectedGameObject(selectButton[0]);

                break;

            case "HowToOperate":

                EventSystem.current.SetSelectedGameObject(selectButton[1]);

                break;

            case "Save":

                EventSystem.current.SetSelectedGameObject(selectButton[2]);

                break;

            case "Titleback":

                EventSystem.current.SetSelectedGameObject(selectButton[3]);

                break;

            case "Setting":

                EventSystem.current.SetSelectedGameObject(selectButton[4]);

                break;

            case "SceneBack":

                EventSystem.current.SetSelectedGameObject(selectButton[5]);

                break;
        }

        openNow = false;
        cancelOK = false;
    }

    public void SavePop()
    {
        savePopPlayable.GetComponent<PlayableDirector>().Play();

        soundManager.OneShotDecisionSound();//決定サウンド
    }

    public void BackTitle()
    {
        titleBackSlide.SetActive(true);

        flagmentData.SceneName = "Title";

        soundManager.OneShotDecisionSound();//決定サウンド
    }

    public void BackScene()
    {
        backSceneSlide.SetActive(true);

        flagmentData.SceneName = "HomeMap";

        soundManager.MusicManager.GetComponent<AudioSource>().Stop();

        soundManager.OneShotDecisionSound();//決定サウンド

        soundManager.LoadingStart_Sound();
    }

    //public void LoadScene()
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
}
