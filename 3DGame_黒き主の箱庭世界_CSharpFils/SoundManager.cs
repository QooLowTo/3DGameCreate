using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �Q�[�����̃T�E���h���Ǘ�����N���X�ł��B
/// </summary>
public class SoundManager : GameManager
{
    private AudioSource adios;

    [SerializeField,Header("BGM��AudioSouce�̎Q�ƃI�u�W�F�N�g")]
    private GameObject musicManager;

    [SerializeField, Header("�v���C���[�̃T�E���h���X�g")]
    private List<AudioClip> player_Sounds = new List<AudioClip>();

    [SerializeField, Header("�v���C���[�̑����T�E���h���X�g")]
    private List<AudioClip> player_Walk_Sounds = new List<AudioClip>();

    [SerializeField, Header("UI�̃T�E���h���X�g")]
    private List<AudioClip> ui_Sounds = new List<AudioClip>();

    [SerializeField, Header("�G�̃T�E���h���X�g")]
    private List<AudioClip> enemy_Sounds = new List<AudioClip>();

    [SerializeField, Header("�{�X�̃T�E���h���X�g")]
    private List<AudioClip> boss_Sounds = new List<AudioClip>();

    [SerializeField, Header("���̑��̃T�E���h���X�g")]
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
    /// �v���C���[�ɂ�����l�X�Ȍ��ʉ���炷���\�b�h�B�����ɂ͌��ʉ����X�g�̔ԍ��̒l����͂��Ă��������B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Player_Sound(int soundNum)
    {
        adios.PlayOneShot(player_Sounds[soundNum]);
    }

    /// <summary>
    /// �v���C���[�̑�����炷���\�b�h�ł��B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Player_Move_Sound(int soundNum)
    {
        adios.PlayOneShot(player_Walk_Sounds[soundNum]);
    }
    /// <summary>
    /// �G�ɂ�����l�X�Ȍ��ʉ���炷���\�b�h�B�����ɂ͌��ʉ����X�g�̔ԍ��̒l����͂��Ă��������B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Enemy_Action_Sound(int soundNum)
    {
        adios.PlayOneShot(enemy_Sounds[soundNum]);
    }
    /// <summary>
    /// �{�X�ɂ�����l�X�Ȍ��ʉ���炷���\�b�h�B�����ɂ͌��ʉ����X�g�̔ԍ��̒l����͂��Ă��������B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Boss_Action_Sound(int soundNum)
    {
        adios.PlayOneShot(boss_Sounds[soundNum]);
    }
    /// <summary>
    /// UI�ɂ�����l�X�Ȍ��ʉ���炷���\�b�h�B�����ɂ͌��ʉ����X�g�̔ԍ��̒l����͂��Ă��������B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_UI_Sound(int soundNum)
    {
        adios.PlayOneShot(ui_Sounds[soundNum]);
    }
    /// <summary>
    /// ���̑��ɂ�����l�X�Ȍ��ʉ���炷���\�b�h�B�����ɂ͌��ʉ����X�g�̔ԍ��̒l����͂��Ă��������B
    /// </summary>
    /// <param name="soundNum"></param>
    public void OneShot_Other_Sound(int soundNum)
    {
        adios.PlayOneShot(other_Sounds[soundNum]);
    }
    /// <summary>
    /// ����{�^�����������ۂɌ��ʉ���炷���\�b�h�ł��B
    /// </summary>
    public void OneShotDecisionSound()
    {
        adios.PlayOneShot(ui_Sounds[4]);
    }
    /// <summary>
    /// �L�����Z���{�^�����������ۂɌ��ʉ���炷���\�b�h�ł��B
    /// </summary>
    public void OneShotCancelSound()
    {
        adios.PlayOneShot(ui_Sounds[5]);
    }
    /// <summary>
    /// ���[�h�J�n����炷���\�b�h�ł��B
    /// </summary>
    public void LoadingStart_Sound()
    {
        adios.PlayOneShot(ui_Sounds[6]);
        adios.PlayOneShot(ui_Sounds[7]);
    }

  
    /// <summary>
    /// BGM���~�߂郁�\�b�h�ł��B
    /// </summary>
    public void BGMStop()
    {
        musicManager.GetComponent<AudioSource>().Stop();
    }
}
