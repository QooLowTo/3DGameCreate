using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
/// <summary>
/// �^�C�����C���̃V�O�i���ŗp����l�X�ȃ��\�b�h���Ǘ�����N���X�ł��B
/// </summary>
public class SignalManager : GameManager
{
    private Player_UI_Controller plaUCon;

    private PlayerInput playerInput;

    private DateSaveUIController dataSaveUIController;

    private SettingBarController settingBar;

    private SoundManager soundManager;

    private GameOverManager gameOverManager;

    private BossGolem bossGolem;

    private ButtonOpenMenu buttonOpen;

    private GameObject findGolem;

    [SerializeField]
    private PlayableDirector openUIPlayable;

    [SerializeField]
    private PlayableDirector gameOverPlayable;

    private CinemachineVirtualCameraBase lockOnCineVir;
    [SerializeField, Header("���b�N�I���J�����̎Q�ƃI�u�W�F�N�g")]
    private GameObject findLookCineVir;

    [SerializeField,Header("�Q�[���I�[�o�[���̃|�X�g�v���Z�X�Q�ƃI�u�W�F�N�g")]
    private GameObject gameOverPost;

    [SerializeField]
    private GameObject continuationButton;

    [SerializeField]
    private GameObject findSoundManager;

    [SerializeField]
    private GameObject findButtonOpen;

    [SerializeField]
    private GameObject findSettingBar;

    [SerializeField]
    private GameObject findGOM;

    [SerializeField]
    private GameObject findDataSave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        StartGetPlayerStatus();

        if (findPla != null)
        {
            plaUCon = findPla.GetComponent<Player_UI_Controller>();

            playerInput = findPla.GetComponent<PlayerInput>();
        }


        if (findButtonOpen != null)
        {
            buttonOpen = findButtonOpen.GetComponent<ButtonOpenMenu>();
        }

        if (findSettingBar != null)
        {
            settingBar = findSettingBar.GetComponent<SettingBarController>();
        }

        if (findLookCineVir != null)
        {
            lockOnCineVir = findLookCineVir.GetComponent<CinemachineVirtualCameraBase>();
        }

        if (findGOM != null)
        {

            gameOverManager = findGOM.GetComponent<GameOverManager>();

        }

        if (findDataSave != null)
        {
            dataSaveUIController = findDataSave.GetComponent<DateSaveUIController>();
        }

        if (findSoundManager != null)
        {
            soundManager = findSoundManager.GetComponent<SoundManager>();
        }
    }

    /// <summary>
    /// �V�O�i���Ŏg�p�BPlayerInput�̃A�N�V�����}�b�v��UI�ɐ؂�ւ��A�Đ����̃^�C�����C�����~�߂�B
    /// </summary>
    public void SiganalOpenedMenu()
    {
        //plaUCon.ChangeActionMap();
        playerInput.SwitchCurrentActionMap("UI");//�����؂�ւ���

        openUIPlayable.Pause();

        plaUCon.CancelStop = false;
    }

    public void SignalMenuClose()
    {
        //plaUCon.MenuClose();
        plaUCon.UiPanel.SetActive(false);

        plaUCon.CineVir.enabled = true;

        playerInput.SwitchCurrentActionMap("Player");

        soundManager.MusicManager.GetComponent<AudioSource>().volume *= 5;

        Time.timeScale = 1f;
    }
    public void SignalGameStart()
    {
       OnGameStart();
    }

    public void SiganalUI_Slide_Sound()
    {
        //soundManager.Adios.PlayOneShot(soundManager.Ui_Sounds[1]);
        soundManager.OneShot_UI_Sound(1);//UI�X���C�h��
    }

    public void SingnalBossActionStart()
    {
        findGolem = GameObject.FindWithTag("Enemy");

        if (findGolem != null)
        {
            bossGolem = findGolem.GetComponent<BossGolem>();
        }

        bossGolem.BossActionStart = false;

        bossGolem.FindBossHPSlide.SetActive(true);
    }

  
    public void SiganalSlidePause()
    {
        buttonOpen.SlidePause();
    }

    public void SingnalSlideClose()
    {
        buttonOpen.SlideClose();
    }

    public void SingnalLoading()
    {
        Loading();
    }

    public void SiganalSaveSettingValue()
    {
        settingBar.SaveSettingValue();
    }

    public void SiganalGameOverPlayableStop()
    {
        //gameOverManager.GameOverPlayableStop();
        if (lockOnCineVir.enabled)
        {
            lockOnCineVir.enabled = false;
        }

        gameOverPlayable.Pause();
        gameOverPost.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(continuationButton);
    }

    public void SiganalAdvisPause()
    {
        dataSaveUIController.AdvisPause();
    }

    public void SinganalAdvisStop()
    {
        dataSaveUIController.AdvisStop();
    }
}
