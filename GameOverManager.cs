using Unity.Cinemachine;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// �Q�[���I�[�o�[���̉��o�𐧌䂷��N���X�ł��B
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector gameOverPlayable;

    [SerializeField]
    private FlagManagementData flagManagementData; // �t���O�Ǘ��f�[�^

    [SerializeField]
    private GameObject post; //���ꉽ�p�H

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
/// �Q�[���I�[�o�[���o���Đ����܂��B
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
/// �Q�[���I�[�o�[���o���Đ����܂��B
/// </summary>
    public void Continuation()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
    
    }

/// <summary>
/// �Q�[���I�[�o�[���o���Đ����܂��B
/// </summary>
    public void BackCenter()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
        flagManagementData.SceneName = "HomeMap";
    }

/// <summary>
/// �Q�[���I�[�o�[���o���Đ����܂��B
/// </summary>
    public void BackTitle()
    {
        gameOverPlayable.Resume();
        gameManager.LoadingStart_Sound();
        flagManagementData.SceneName = "Title";
    }
}
