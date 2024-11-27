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
public class TitleIvent : MonoBehaviour
{
    [SerializeField]
    private List<PlayableDirector> titlePlayables = new List<PlayableDirector>();

    [SerializeField]
    private PlayerInput plain;//インプットシステム

    [SerializeField]
    private EventSystem eve;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField]
    private GameObject biginCancelButton;

    [SerializeField]
    private GameObject biggingButton;

    //[SerializeField] 
    //private GameObject titleButtons;

    [SerializeField]
    private GameObject mainEveSys;

    [SerializeField]
    private GameObject subEveSys;

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private GameObject findGame;

    [SerializeField]
    private FlagmentData gameData;

    [SerializeField]
    private string loadSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plain = GetComponent<PlayerInput>();

        GM = findGame.GetComponent<GameManager>();

        eve = EventSystem.current;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Begging()
    {
        GM.Decision_Sound();
        mainEveSys.SetActive(false);
        titlePlayables[0].Play();
    }

    public void BeggingPause()
    {
        subEveSys.SetActive(true);
        //titleButtons.SetActive(false);
        titlePlayables[0].Pause(); 
    }

    public void BeggingResum()
    {
        GM.Cancel_Sound();
        //titleButtons.SetActive(true);
    
        titlePlayables[0].Resume();
    }

    public void EventActive()
    { 
    eve.firstSelectedGameObject = biggingButton;
        subEveSys.SetActive(false);
        mainEveSys.SetActive(true);
    }

    public void Begin()
    {
        gameData.SceneName = loadSceneName;
        GM.MusicManager.GetComponent<AudioSource>().Stop();
        GM.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[1].Play();
    }

    public void Loding()
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
}
