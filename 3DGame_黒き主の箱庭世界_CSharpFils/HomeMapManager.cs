using UnityEngine;
/// <summary>
/// バトル外でのゲームの進行等を管理するクラスです。
/// </summary>
public class HomeMapManager : GameManager
{
    private Player_Rest_Controller playerRestCon;

    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

    void Start()
    {

        StartGameSetting();

        StartGetPlayerStatus();

        playerRestCon = findPla.GetComponent<Player_Rest_Controller>();


        if (startUI != null && !debugMode)
        {

            startUI.SetActive(true);

            //マジックナンバー
            soundManager.OneShot_UI_Sound(7); //ロード完了音
        }

        if (debugMode)
        {

            StartCoroutine(DebugGameStart());

        }
    }
   
}
