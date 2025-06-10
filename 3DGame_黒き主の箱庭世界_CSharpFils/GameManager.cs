using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム中の進行等を管理する子クラスの親クラスです。
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField,Header("デバッグモード(スタート演出を無視して開始できる。)")]
    protected bool debugMode = false;

    protected Player_Status_Controller playerStatCon;

    protected Player_Experience_Manager playerEXPManager;

    [SerializeField]
    protected GameObject findPlayer;

    [SerializeField]
    protected GameObject startUI;

    protected Gamepad gamepad;


    [SerializeField]
    protected SettingData settingData;

    [SerializeField]
    protected StatusDate statusData;

    [SerializeField]
    protected ExpManager eXPManager;


    [SerializeField]
    protected CinemachineInputAxisController cameraAxis;
  
    public CinemachineInputAxisController CameraAxis { get => cameraAxis; set => cameraAxis = value; }


    protected void StartGameSetting()
    {
        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }   

        cameraAxis.GetComponent<CinemachineInputAxisController>();

        Application.targetFrameRate = settingData.FrameRate;

        gamepad = Gamepad.current;

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
   
    /// <summary>
    /// プレイヤーのオブジェクトを参照し、プレイヤーのステータスクラスとExpクラスをゲットするメソッドです。
    /// </summary>
    protected void StartGetPlayerStatus()
    {
        Debug.Log(findPlayer);

        playerStatCon = findPlayer.GetComponent<Player_Status_Controller>();

        playerEXPManager = findPlayer.GetComponent<Player_Experience_Manager>();

    }

    /// <summary>
    /// シグナルで使用。このメソッドが呼ばれるとプレイヤーの操作できるようになる。
    /// </summary>
    /// 
    public void OnGameStart()
    {

        findPlayer.GetComponent<Player>().GameStart = true;

        playerStatCon.HealthBar.SetActive(true);

        playerStatCon.SetLiveHP();

        cameraAxis.enabled = true;

   

        findPlayer.GetComponent<CharacterController>().enabled = true;

        Debug.Log(findPlayer.GetComponent<CharacterController>().enabled);

        findPlayer.GetComponent<PlayerInput>().enabled = true;
    }

    protected IEnumerator DebugGameStart()
    {
        yield return new WaitForSeconds(0.1f);

        OnGameStart();
    }

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
