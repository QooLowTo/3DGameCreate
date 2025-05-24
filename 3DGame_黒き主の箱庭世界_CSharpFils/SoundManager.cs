using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム中のサウンドを管理するクラスです。
/// </summary>
public class SoundManager : GameManager
{
    private AudioSource adios;

    [SerializeField,Header("BGMのAudioSouceの参照オブジェクト")]
    private GameObject musicManager;

    [SerializeField, Header("プレイヤーのサウンドリスト")]
    private List<AudioClip> player_Sounds = new List<AudioClip>();

    [SerializeField, Header("プレイヤーの足音サウンドリスト")]
    private List<AudioClip> player_Walk_Sounds = new List<AudioClip>();

    [SerializeField, Header("UIのサウンドリスト")]
    private List<AudioClip> ui_Sounds = new List<AudioClip>();

    [SerializeField, Header("敵のサウンドリスト")]
    private List<AudioClip> enemy_Sounds = new List<AudioClip>();

    [SerializeField, Header("ボスのサウンドリスト")]
    private List<AudioClip> boss_Sounds = new List<AudioClip>();

    [SerializeField, Header("その他のサウンドリスト")]
    protected List<AudioClip> other_Sounds = new List<AudioClip>();

    public GameObject MusicManager { get => musicManager; set => musicManager = value; }
    public AudioSource Adios { get => adios; set => adios = value; }
    public List<AudioClip> Ui_Sounds { get => ui_Sounds; set => ui_Sounds = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        adios = gameObject.GetComponent<AudioSource>();

        gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;

        musicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume;
    }
    /// <summary>
    /// プレイヤーにおける様々な効果音を鳴らすメソッド。引数には効果音リストの番号の値を入力してください。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Player_Sound(int soundNum)
    {
        adios.PlayOneShot(player_Sounds[soundNum]);
    }

    /// <summary>
    /// プレイヤーの足音を鳴らすメソッドです。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Player_Move_Sound(int soundNum)
    {
        adios.PlayOneShot(player_Walk_Sounds[soundNum]);
    }
    /// <summary>
    /// 敵における様々な効果音を鳴らすメソッド。引数には効果音リストの番号の値を入力してください。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Enemy_Action_Sound(int soundNum)
    {
        adios.PlayOneShot(enemy_Sounds[soundNum]);
    }
    /// <summary>
    /// ボスにおける様々な効果音を鳴らすメソッド。引数には効果音リストの番号の値を入力してください。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Boss_Action_Sound(int soundNum)
    {
        adios.PlayOneShot(boss_Sounds[soundNum]);
    }
    /// <summary>
    /// UIにおける様々な効果音を鳴らすメソッド。引数には効果音リストの番号の値を入力してください。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_UI_Sound(int soundNum)
    {
        adios.PlayOneShot(ui_Sounds[soundNum]);
    }
    /// <summary>
    /// その他における様々な効果音を鳴らすメソッド。引数には効果音リストの番号の値を入力してください。
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Other_Sound(int soundNum)
    {
        adios.PlayOneShot(other_Sounds[soundNum]);
    }
    /// <summary>
    /// 決定ボタンを押した際に効果音を鳴らすメソッドです。
    /// </summary>
    public void OneShotDecisionSound()
    {
        adios.PlayOneShot(ui_Sounds[4]);
    }
    /// <summary>
    /// キャンセルボタンを押した際に効果音を鳴らすメソッドです。
    /// </summary>
    public void OneShotCancelSound()
    {
        adios.PlayOneShot(ui_Sounds[5]);
    }
    /// <summary>
    /// ロード開始音を鳴らすメソッドです。
    /// </summary>
    public void LoadingStart_Sound()
    {
        adios.PlayOneShot(ui_Sounds[6]);
        adios.PlayOneShot(ui_Sounds[7]);
    }

  
    /// <summary>
    /// BGMを止めるメソッドです。
    /// </summary>
    public void BGMStop()
    {
        musicManager.GetComponent<AudioSource>().Stop();
    }
}
