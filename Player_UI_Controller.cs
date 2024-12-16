
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

/// <summary>
/// �v���C���[�֘AUI���Ǘ�����N���X
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
    private PlayerInput playerInput;//�C���v�b�g�V�X�e��

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

            musicManager.GetComponent<AudioSource>().volume /= 5; //�}�W�b�N�i���o�[�����I�ϐ����E�萔���E�R�����g�c���Ă�������

            gameManager.Menu_Open_Sound();

            UIPanel.SetActive(true);//UI�p�l����\��

            playable.Play();//UI�^�C�����C���A�j���[�V�����Đ�

            cineVir.enabled = false;//�J�������~�߂�

            openMenu = true;
        
            Time.timeScale = 0f;
        
    }

    public void ChangeActionMap()//�V�O�i���Ő���
    {
        playerInput.SwitchCurrentActionMap("UI");//�����؂�ւ���
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

    public void MenuClose()//�V�O�i���Ő���
    {  
        UIPanel.SetActive(false);

        cineVir.enabled = true;

        playerInput.SwitchCurrentActionMap("Player");

        musicManager.GetComponent<AudioSource>().volume *= 5; //�}�W�b�N�i���o�[�����I�ϐ����E�萔���E�R�����g�c���Ă�������

        Time.timeScale = 1f;
    }

}
