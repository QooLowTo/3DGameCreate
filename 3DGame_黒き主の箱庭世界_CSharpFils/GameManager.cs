using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム中のエフェクト、サウンドなどの演出を制御する子クラスの親クラスです。
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField,Header("デバッグモード(スタート演出を無視して開始できる。)")]
    protected bool debugMode = false;

    //[SerializeField]
    //protected bool gameStart = false;//ゲームを開始できるようにする。

    protected Player_Status_Controller plaSta;

    protected Player_Experience_Manager plaExp;

    [SerializeField]
    protected GameObject findPla;

    [SerializeField]
    protected GameObject startUI;

    protected Gamepad gamepad;

    //[SerializeField]
    //protected GameObject musicManager;

    [SerializeField]
    protected SettingData settingData;

    [SerializeField]
    protected StatusDate statusDate;

    [SerializeField]
    protected ExpManager expManager;

    //protected AudioSource adios;

    [SerializeField]
    protected CinemachineInputAxisController cameraAxis;

    //[SerializeField]
    //protected List<AudioClip> player_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> player_Walk_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> ui_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> other_Sounds = new List<AudioClip>();

   

  
    public CinemachineInputAxisController CameraAxis { get => cameraAxis; set => cameraAxis = value; }


    protected void StartGameSetting()
    {
        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }

        //adios = gameObject.GetComponent<AudioSource>();

        //gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;

        //musicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume;

        cameraAxis.GetComponent<CinemachineInputAxisController>();

        Application.targetFrameRate = settingData.FrameRate;

        gamepad = Gamepad.current;

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    //protected void DebugGameStart()
    //{
    //    gameStart = true;

    //    plaSta.HealthBar.SetActive(true);

    //    plaSta.SetLiveHP();

    //    cameraAxis.enabled = true;

    //    StartCoroutine();
    //}

   
    /// <summary>
    /// プレイヤーのオブジェクトを参照し、プレイヤーのステータスクラスとExpクラスをゲットするメソッドです。
    /// </summary>
    protected void StartGetPlayerStatus()
    {
        Debug.Log(findPla);

        plaSta = findPla.GetComponent<Player_Status_Controller>();

        plaExp = findPla.GetComponent<Player_Experience_Manager>();

        //plaSta.HealthBar.SetActive(true);
    }

    /// <summary>
    /// シグナルで使用。このメソッドが呼ばれるとプレイヤーの操作できるようになる。
    /// </summary>
    /// 
    public void OnGameStart()
    {

        //gameStart = true;

        findPla.GetComponent<Player>().GameStart = true;

        plaSta.HealthBar.SetActive(true);

        plaSta.SetLiveHP();

        cameraAxis.enabled = true;

   

        findPla.GetComponent<CharacterController>().enabled = true;

        Debug.Log(findPla.GetComponent<CharacterController>().enabled);

        findPla.GetComponent<PlayerInput>().enabled = true;
    }

    protected IEnumerator DebugGameStart()
    {
        yield return new WaitForSeconds(0.1f);

        OnGameStart();
    }

    //public void OneShotPlayerSound(int soundNum)
    //{
    //    adios.PlayOneShot(player_Sounds[soundNum]);
    //}

    //public void OneShotPlayerMoveSound(int soundNum)
    //{
    //    adios.PlayOneShot(player_Walk_Sounds[soundNum]);
    //}


    //public void Decision_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[4]);
    //}
    //public void Cancel_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[5]);
    //}


    //public void Menu_Open_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[0]);
    //}

    //public void Select_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[2]);
    //}

    //public void Select_Ceiling_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[3]);
    //}

    //public void UI_Slide_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[1]);
    //}

    //public void LoadingStart_Sound()
    //{
    //    adios.PlayOneShot(ui_Sounds[6]);
    //    adios.PlayOneShot(ui_Sounds[7]);
    //}

    //public void Get_Weapon_Sound()
    //{
    //    adios.PlayOneShot(other_Sounds[0]);
    //}

    //public void Portal_Appearance_Sound()
    //{
    //    adios.PlayOneShot(other_Sounds[1]);
    //}

    //public void BGMStop()
    //{ 
    //musicManager.GetComponent<AudioSource>().Stop();
    //}


    /// <summary>
    /// ローディング画面に移動するためのメソッドです。シグナルで使用。
    /// </summary>
    public void Loading()
    {
       
        StartCoroutine(LoadSceneAsync("LoadingScene"));

        IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }



    /// <summary>
    /// ゲームパッドを振動させます。引数には「低周波(左)」、「高周波(右)」、「時間」を代入してください。
    /// </summary>
    /// <param name="low"></param>
    /// <param name="high"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator GamePadVibration(float low,float high,float time)
    { 
        gamepad.SetMotorSpeeds(low,high);
        yield return new WaitForSeconds(time);
        gamepad.SetMotorSpeeds(0.0f, 0.0f);//振動停止
    }

    protected IEnumerator Boss_Kill_GamePadVibration(float low, float high, float time)
    {
        gamepad.SetMotorSpeeds(low, high);
        yield return new WaitForSeconds(time);
        gamepad.SetMotorSpeeds(0.0f, 0.0f);//振動停止
    }
}
