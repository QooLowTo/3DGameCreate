
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

public class Player_UI_Controller : MonoBehaviour
{
    private bool openMenu = false;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private PlayableDirector playable;

    [SerializeField]
    private CinemachineVirtualCameraBase cineVir;
    [SerializeField]
    private GameObject findCine;

    [SerializeField]
    private PlayerInput plain;//インプットシステム

    [SerializeField]
    private ButtonOpen buttonOpen;
    [SerializeField]
    private GameObject Findbutton;

    [SerializeField]
    private Player_Battle_Controller plabat;

    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject FindGM;

    [SerializeField]
    private GameObject musicManager;

  

    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);

        cineVir = findCine.GetComponent<CinemachineVirtualCameraBase>();
        plain = gameObject.GetComponent<PlayerInput>();
        plabat = gameObject.GetComponent<Player_Battle_Controller>();
        buttonOpen = Findbutton.GetComponent<ButtonOpen>();
        GM = FindGM.GetComponent<GameManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (plain.actions["Cancel"].triggered)
        {
            Cancel();
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
            if (openMenu||plabat.GameStarting == false||plabat.GameOver) return;

            musicManager.GetComponent<AudioSource>().volume /= 5;

            GM.Menu_Open_Sound();

            uiPanel.SetActive(true);//UIパネルを表示

            playable.Play();//UIタイムラインアニメーション再生

            cineVir.enabled = false;//カメラを止める

            openMenu = true;
        
            Time.timeScale = 0f;
        
    }

    public void ChangeActionMap()//シグナルで制御
    {
        plain.SwitchCurrentActionMap("UI");//操作を切り替える
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

        GM.Cancel_Sound();

        playable.Resume();

        openMenu = false;

    }

    public void MenuClose()//シグナルで制御
    {  
        uiPanel.SetActive(false);

        cineVir.enabled = true;

        plain.SwitchCurrentActionMap("Player");

        musicManager.GetComponent<AudioSource>().volume *= 5;

        Time.timeScale = 1f;
    }

}
