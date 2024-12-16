using Fungus;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���C��UI�̐ݒ荀�ڂ̃X���C�_�[�Ŋe��ݒ肪�ł���悤�ɂ���N���X�ł��B
/// </summary>
public class SettingBarController : MonoBehaviour
{
    [SerializeField]
    private SettingData settingData;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGM; //���̃N���X���Ƃ��̕\�L�Ȃ̂ŁA���킹�܂����B

    [SerializeField]
    private List<Slider> settingSliderList = new List<Slider>();

    [SerializeField]
    private List<TextMeshProUGUI> sliderValues = new List<TextMeshProUGUI>();

    private float bgmValue;
    private float nowBgmValue;

    private float soundValue;

    private int frameRateValue;

    void Start()
    {
        gameManager = findGM.GetComponent<GameManager>();
     

        settingSliderList[0].value = settingData.BgmVolume * 10;
        settingSliderList[1].value = settingData.SoundVolum * 10;
        settingSliderList[2].value = settingData.FrameRate / 60;

        if (settingSliderList[2].value < 1)
        {
            settingSliderList[2].value = 0;
        }

        //�����̃}�W�b�N�i���o�[��萔�ɂ��Ă�������

    }

 
    private void Update()
    {
        bgmValue = settingSliderList[0].value / 10;
        soundValue = settingSliderList[1].value / 10;
        frameRateValue = (int)settingSliderList[2].value * 60;

        if ((int)settingSliderList[2].value * 60 < 1)
        {
            frameRateValue = 30;
        }

        gameManager.GetComponent<AudioSource>().volume = soundValue;

        sliderValues[0].text = (bgmValue * 10).ToString();
        sliderValues[1].text = (soundValue * 10).ToString();
        sliderValues[2].text = frameRateValue.ToString();

        //�}�W�b�N�i���o�[��萔�ɂ��Ă�������
    }

    public void SaveSettingValue()
    { 
        settingData.BgmVolume = bgmValue;
        settingData.SoundVolum = soundValue;
        settingData.FrameRate = frameRateValue;

        nowBgmValue = gameManager.MusicManager.GetComponent<AudioSource>().volume;

        if (bgmValue != nowBgmValue)
        { 
            gameManager.MusicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume / 5;
            //�}�W�b�N�i���o�[�����I�ϐ����@OR�@�萔���@OR�@�R�����g�c���Ă�������
        }
       
        gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolume;

        Application.targetFrameRate = settingData.FrameRate;
    }
}
