using Unity.Cinemachine;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// ゲームオーバー時の演出を制御するクラスです。
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector gameOverPlayable;

    [SerializeField]
    private FlagManagementData flagManagementData; // フラグ管理データ

    [SerializeField]
    private GameObject post; //これ何用？

    [SerializeField]
    private CinemachineVirtualCameraBase lockOnCineVir;

    [SerializeField]
    private GameObject findLookCineVir;

    [SerializeField]
    private GameObject mainEvent;
    [SerializeField]
    private GameObject gameOverEvent;

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject findGM;

    void Start()
    {
        gameManager = findGM.GetComponent<GameManager>();
        lockOnCineVir = findLookCineVir.GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
        if (lockOnCineVir.enabled)
        { 
        lockOnCineVir.enabled = false;
        }
    }

/// <summary>
/// ゲームオーバー演出を再生します。
/// </summary>
    public void GameOverPlayableStop()
    { 
        gameOverPlayable.Pause();
        post.SetActive(true);
        Time.timeScale = 0f;
        mainEvent.SetActive(false);
        gameOverEvent.SetActive(true);
    }

/// <summary>
/// ゲームオーバー演出を再生します。
/// </summary>
    public void Continuation()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
    
    }

/// <summary>
/// ゲームオーバー演出を再生します。
/// </summary>
    public void BackCenter()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
        flagManagementData.SceneName = "HomeMap";
    }

/// <summary>
/// ゲームオーバー演出を再生します。
/// </summary>
    public void BackTitle()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
        flagManagementData.SceneName = "Title";
    }
}
