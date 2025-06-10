using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// ゲームオーバー時の演出を制御するクラスです。
/// </summary>
public class GameOverManager : GameManager
{
    [SerializeField]
    private PlayableDirector gameOverPlayable;

    [SerializeField]
    private FlagManagementData flagManagementData;

    [SerializeField,Header("タイトルバック演出用オブジェクト")]
    private GameObject titleback;

    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

    void Start()
    {
        soundManager = findSoundManager.GetComponent<SoundManager>();
     
    }

   
    /// <summary>
    /// ボタンで使用。コンテニューボタン。
    /// </summary>
    public void Continuation()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
    
    }
    /// <summary>
    /// ボタンで使用。押すとHomeMapへ戻る。
    /// </summary>
    public void BackCenter()
    {
        gameOverPlayable.Resume();
        soundManager.LoadingStart_Sound();
        flagManagementData.SceneName = "HomeMap";
    }
    /// <summary>
    /// ボタンで使用。押すとタイトルへ戻る。
    /// </summary>
    public void BackTitle()
    {
       titleback.SetActive(true);
        soundManager.OneShotDecisionSound();//決定サウンド
        flagManagementData.SceneName = "Title";
    }
}
