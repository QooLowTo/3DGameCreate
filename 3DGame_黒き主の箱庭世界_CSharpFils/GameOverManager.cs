using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// ゲームオーバー時の演出を制御するクラスです。
/// </summary>
public class GameOverManager : GameManager
{
    [SerializeField]
    private PlayableDirector gameOverPlayable;

    [SerializeField]
    private FlagManagementData flagmentData;

    //[SerializeField]
    //private GameObject post;

    [SerializeField,Header("タイトルバック演出用オブジェクト")]
    private GameObject titleback;

    //[SerializeField] 
    //private GameObject continuationButton;

    //[SerializeField,Header("ロックオンカメラ")]
    //private CinemachineVirtualCameraBase lockOnCineVir;
    //[SerializeField]
    //private GameObject findLookCineVir;

    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManger;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundManager = findSoundManger.GetComponent<SoundManager>();
     
    }

    // Update is called once per frame


    //public void GameOverPlayableStop()
    //{ 
    //    gameOverPlayable.Pause();
    //    post.SetActive(true);
    //    Time.timeScale = 0f;
    //    EventSystem.current.SetSelectedGameObject(continuationButton);
    //}
    /// <summary>
    /// ボタンで使用。コンテニューボタン。
    /// </summary>
    public void Continuation()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
    
    }
    /// <summary>
    /// ボタンで使用。押すとHomeMapへ戻る。
    /// </summary>
    public void BackCenter()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
        flagmentData.SceneName = "HomeMap";
    }
    /// <summary>
    /// ボタンで使用。押すとタイトルへ戻る。
    /// </summary>
    public void BackTitle()
    {
       titleback.SetActive(true);
        soundManager.OneShotDecisionSound();//決定サウンド
        flagmentData.SceneName = "Title";
    }
}
