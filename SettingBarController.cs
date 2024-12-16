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
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGM; //他のクラスだとこの表記なので、合わせました。

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

        //これらのマジックナンバーを定数にしてください

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

        //マジックナンバーを定数にしてください
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
            //マジックナンバー発見！変数化　OR　定数化　OR　コメント残してください
        }
       
        gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolume;

        Application.targetFrameRate = settingData.FrameRate;
    }
}
