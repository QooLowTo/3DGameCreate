
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

/// <summary>
/// プレイヤー関連UIを管理するクラス
/// </summary>
public class Player_UI_Controller : MonoBehaviour
{
    private bool openMenu = false;

    [SerializeField]
    private GameObject UIPanel;

    [SerializeField]
    private PlayableDirector playable;

    [SerializeField]
    private CinemachineVirtualCameraBase cineVir;
    [SerializeField]
    private GameObject findCine;

    [SerializeField]
    private PlayerInput playerInput;//インプットシステム

    [SerializeField]
    private ButtonOpen buttonOpen;
    [SerializeField]
    private GameObject findbutton;

    [SerializeField]
    private Player_Battle_Controller playerBCon;

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject findGM;

    [SerializeField]
    private GameObject musicManager;

  
    void Start()
    {
        UIPanel.SetActive(false);

        cineVir = findCine.GetComponent<CinemachineVirtualCameraBase>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerBCon = gameObject.GetComponent<Player_Battle_Controller>();
        buttonOpen = findbutton.GetComponent<ButtonOpen>();
        gameManager = findGM.GetComponent<GameManager>();
       
    }

    void Update()
    {
        if (playerInput.actions["Cancel"].triggered)
        {
            Cancel();
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
            if (openMenu||playerBCon.GameStarting == false||playerBCon.GameOver) return;

            musicManager.GetComponent<AudioSource>().volume /= 5; //マジックナンバー発見！変数化・定数化・コメント残してください

            gameManager.Menu_Open_Sound();

            UIPanel.SetActive(true);//UIパネルを表示

            playable.Play();//UIタイムラインアニメーション再生

            cineVir.enabled = false;//カメラを止める

            openMenu = true;
        
            Time.timeScale = 0f;
        
    }

    public void ChangeActionMap()//シグナルで制御
    {
        playerInput.SwitchCurrentActionMap("UI");//操作を切り替える
        playable.Pause();
    }

    public void Cancel(/*InputAction.CallbackContext context*/)
    {
        if (buttonOpen.CancelOK)
        { 

            buttonOpen.SlideResum();
          
            buttonOpen.CancelOK = false;

        }

       

        if (openMenu == false || buttonOpen.OpenNow) return;

        gameManager.Cancel_Sound();

        playable.Resume();

        openMenu = false;

    }

    public void MenuClose()//シグナルで制御
    {  
        UIPanel.SetActive(false);

        cineVir.enabled = true;

        playerInput.SwitchCurrentActionMap("Player");

        musicManager.GetComponent<AudioSource>().volume *= 5; //マジックナンバー発見！変数化・定数化・コメント残してください

        Time.timeScale = 1f;
    }

}
