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
    private FlagmentData flagmentData;

    [SerializeField]
    private GameObject post;

    [SerializeField]
    private CinemachineVirtualCameraBase lockOnCineVir;
    [SerializeField]
    private GameObject findLookCineVir;

    [SerializeField]
    private GameObject mainEve;
    [SerializeField]
    private GameObject gameOverEve;

    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject findGM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = findGM.GetComponent<GameManager>();
        lockOnCineVir = findLookCineVir.GetComponent<CinemachineVirtualCameraBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockOnCineVir.enabled)
        { 
        lockOnCineVir.enabled = false;
        }
    }

    public void GameOverPlayableStop()
    { 
        gameOverPlayable.Pause();
        post.SetActive(true);
        Time.timeScale = 0f;
        mainEve.SetActive(false);
        gameOverEve.SetActive(true);
    }

    public void Continuation()
    {
        gameOverPlayable.Resume();
        GM.LoadingStart_Sound();
    
    }

    public void BackCenter()
    {
        gameOverPlayable.Resume();
        GM.LoadingStart_Sound();
        flagmentData.SceneName = "HomeMap";
    }

    public void BackTitle()
    {
        gameOverPlayable.Resume();
        GM.LoadingStart_Sound();
        flagmentData.SceneName = "Title";
    }
}
