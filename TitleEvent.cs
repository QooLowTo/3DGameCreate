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
public class TitleEvent : MonoBehaviour
{
    [SerializeField]
    private List<PlayableDirector> titlePlayables = new List<PlayableDirector>();

    [SerializeField]
    private PlayerInput playerInput;//インプットシステム

    [SerializeField]
    private EventSystem eve;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField]
    private GameObject biginCancelButton; //何用？説明してください

    [SerializeField]
    private GameObject biggingButton;　//何用？説明してください

    //[SerializeField] 
    //private GameObject titleButtons;

    [SerializeField]
    private GameObject mainEveSys;

    [SerializeField]
    private GameObject subEveSys;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGame;

    [SerializeField]
    private FlagManagementData
     gameData;

    [SerializeField]
    private string loadSceneName;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        gameManager = findGame.GetComponent<GameManager>();

        eve = EventSystem.current;
    }

    /// <summary>
    /// イベントを開始します。
    /// </summary>
    public void Begging() //Beggingだとスペルミスしているので、Begging→Beginに変更してください
    {
        gameManager.Decision_Sound();
        mainEveSys.SetActive(false);
        titlePlayables[0].Play();
    }

    /// <summary>
    /// イベントを一時停止します。
    /// </summary>
    public void BeggingPause() //BeginPauseにした方がいい。それか、BeginningPause
    {
        subEveSys.SetActive(true);
        //titleButtons.SetActive(false);
        titlePlayables[0].Pause(); 
    }

    /// <summary>
    /// イベントを再開します。
    /// </summary>
    public void BeggingResum() //BeginResumeもしくはBeginningResume
    {
        gameManager.Cancel_Sound();
        //titleButtons.SetActive(true);
    
        titlePlayables[0].Resume();
    }

    /// <summary>
    /// イベントをアクティブにします。
    /// </summary>
    public void EventActive()
    { 
        eve.firstSelectedGameObject = biggingButton;
        subEveSys.SetActive(false);
        mainEveSys.SetActive(true);
    }

    /// <summary>
    /// ゲームを開始します。
    /// </summary>
    public void Begin()
    {
        gameData.SceneName = loadSceneName;
        gameManager.MusicManager.GetComponent<AudioSource>().Stop();
        gameManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[1].Play();
    }


        /// <summary>
        /// シーンをロードします。
        /// </summary>
    public void Loading()
    {
        StartCoroutine(LoadSceneAsync("LoadingScene"));
    }

    /// <summary>
    /// シーンを非同期でロードします。
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
}
