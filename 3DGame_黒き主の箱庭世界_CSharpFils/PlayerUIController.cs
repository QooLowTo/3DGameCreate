
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

/// <summary>
/// メニューUIを開けるようにし、メニュー中でのキャンセルを行えるようにするクラスです。
/// </summary>
public class PlayerUIController : MonoBehaviour
{
    private bool openMenu = false;

    private bool cancelStop = false;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private PlayableDirector playable;

  
    private CinemachineVirtualCameraBase cineVir;
    [SerializeField]
    private GameObject findCine;

    private Player player;

  
    private PlayerInput playerInput;

    private ButtonOpenMenu buttonOpenMenu;
    [SerializeField]
    private GameObject findbutton;


    private DateSaveUIController dataSaveUIController;
    [SerializeField]
    private GameObject findSavebutton;


    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

  
    public bool CancelStop { get => cancelStop; set => cancelStop = value; }
    public GameObject UiPanel { get => uiPanel; set => uiPanel = value; }
    public CinemachineVirtualCameraBase CineVir { get => cineVir; set => cineVir = value; }


    void Start()
    {
        uiPanel.SetActive(false);

        cineVir = findCine.GetComponent<CinemachineVirtualCameraBase>();

        player = GetComponent<Player>();

        playerInput = gameObject.GetComponent<PlayerInput>();

        buttonOpenMenu = findbutton.GetComponent<ButtonOpenMenu>();

        dataSaveUIController = findSavebutton.GetComponent<DateSaveUIController>();


        soundManager = findSoundManager.GetComponent<SoundManager>();
     
    }

    void Update()
    {
        if (openMenu) return;

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    /// <summary>
    /// PlayerInputのイベントで使用。メニューを開き、ゲームをポーズ状態にするメソッドです。。
    /// </summary>
    /// <param name="context"></param>
    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed || openMenu || player.GameOver /*|| battleManager.BossBattle*/) return;

        openMenu = true;

        cancelStop = true;

        soundManager.MusicManager.GetComponent<AudioSource>().volume /= 5;

        soundManager.OneShot_UI_Sound(0);//UIが開く音

        uiPanel.SetActive(true);//UIパネルを表示

        playable.Play();//UIタイムラインアニメーション再生

        cineVir.enabled = false;//カメラを止める

        EventSystem.current.SetSelectedGameObject(buttonOpenMenu.SelectButton[0]);

        Time.timeScale = 0f;

    }
    /// <summary>
    /// PlayerInputのイベントで使用。メニューでの選択をキャンセルするメソッドです。
    /// </summary>
    /// <param name="context"></param>
    public void Cancel(InputAction.CallbackContext context)
    {
        if (!context.performed || cancelStop) return;

        if (dataSaveUIController.SaveAdvisOpen)
        {
            dataSaveUIController.AdvisResume();
        }

        if (buttonOpenMenu.CancelOK&& dataSaveUIController.SaveAdvisOpen == false)
        { 
            buttonOpenMenu.SlideResum();
        }

        if (!openMenu || buttonOpenMenu.OpenNow) return;

        soundManager.OneShotCancelSound();//キャンセルサウンド

        playable.Resume();

        openMenu = false;

    }

}
