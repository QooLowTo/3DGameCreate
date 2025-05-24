using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// �Q�[���I�[�o�[���̉��o�𐧌䂷��N���X�ł��B
/// </summary>
public class GameOverManager : GameManager
{
    [SerializeField]
    private PlayableDirector gameOverPlayable;

    [SerializeField]
    private FlagManagementData flagmentData;

    //[SerializeField]
    //private GameObject post;

    [SerializeField,Header("�^�C�g���o�b�N���o�p�I�u�W�F�N�g")]
    private GameObject titleback;

    //[SerializeField] 
    //private GameObject continuationButton;

    //[SerializeField,Header("���b�N�I���J����")]
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
    /// �{�^���Ŏg�p�B�R���e�j���[�{�^���B
    /// </summary>
    public void Continuation()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
    
    }
    /// <summary>
    /// �{�^���Ŏg�p�B������HomeMap�֖߂�B
    /// </summary>
    public void BackCenter()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
        flagmentData.SceneName = "HomeMap";
    }
    /// <summary>
    /// �{�^���Ŏg�p�B�����ƃ^�C�g���֖߂�B
    /// </summary>
    public void BackTitle()
    {
       titleback.SetActive(true);
        soundManager.OneShotDecisionSound();//����T�E���h
        flagmentData.SceneName = "Title";
    }
}
