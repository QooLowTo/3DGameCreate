using Fungus;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインUIの設定項目のスライダーで各種設定ができるようにするクラスです。
/// </summary>
public class SettingBarController : MonoBehaviour
{
    [SerializeField]
    private SettingData settingData;

    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private GameObject findGameManager;

    [SerializeField]
    private List<Slider> settingSliderList = new List<Slider>();

    [SerializeField]
    private List<TextMeshProUGUI> sliderValues = new List<TextMeshProUGUI>();

    private float bgmValue;
    private float nowBgmValue;

    private float soundValue;

    private int frameRateValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = findGameManager.GetComponent<GameManager>();
     

        settingSliderList[0].value = settingData.BgmVolume * 10;
        settingSliderList[1].value = settingData.SoundVolum * 10;
        settingSliderList[2].value = settingData.FrameRate / 60;

        if (settingSliderList[2].value < 1)
        {
            settingSliderList[2].value = 0;
        }


    }

    // Update is called once per frame
 
    private void Update()
    {
        bgmValue = settingSliderList[0].value / 10;
        soundValue = settingSliderList[1].value / 10;
        frameRateValue = (int)settingSliderList[2].value * 60;

        if ((int)settingSliderList[2].value * 60 < 1)
        {
            frameRateValue = 30;
        }

        GM.GetComponent<AudioSource>().volume = soundValue;

        sliderValues[0].text = (bgmValue * 10).ToString();
        sliderValues[1].text = (soundValue * 10).ToString();
        sliderValues[2].text = frameRateValue.ToString();
    }

    public void SaveSettingValue()
    { 
        settingData.BgmVolume = bgmValue;
        settingData.SoundVolum = soundValue;
        settingData.FrameRate = frameRateValue;

        nowBgmValue = GM.MusicManager.GetComponent<AudioSource>().volume;

        if (bgmValue != nowBgmValue)
        { 
        GM.MusicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume / 5;
        }
       
        gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;

        Application.targetFrameRate = settingData.FrameRate;
    }
}
