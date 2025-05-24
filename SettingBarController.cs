using Fungus;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ���C��UI�̐ݒ荀�ڂ̃X���C�_�[�Ŋe��ݒ肪�ł���悤�ɂ���N���X�ł��B
/// </summary>
public class SettingBarController : UIManager
{
    //[SerializeField]
    //private SettingData settingData;

    //private UIManager uiManager;

    //private GameObject findUiManager;

    [SerializeField]
    private List<Slider> settingSliderList = new List<Slider>();

    [SerializeField]
    private List<TextMeshProUGUI> settingValues = new List<TextMeshProUGUI>();

   
    //[SerializeField]
    //private PlayerInput plain;
    //[SerializeField]
    //private GameObject findPlain;

    private float bgmValue;

    private float nowBgmValue;

    private float soundValue;

    private int frameRateValue;

    private float cameraRotateSpeed;

    private bool vibrationOn = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUISetting();

        settingSliderList[0].value = settingData.BgmVolume * 10;

        settingSliderList[1].value = settingData.SoundVolum * 10;

        settingSliderList[2].value = settingData.FrameRate / 60;

        settingSliderList[3].value = settingData.CameraRotateSpeed * 10;


        if (settingSliderList[2].value < 1)
        {
            settingSliderList[2].value = 0;
        }

       

        settingData.VibrationON = vibrationOn;

        settingValues[0].text = (bgmValue * 10).ToString();

        settingValues[1].text = (soundValue * 10).ToString();

        settingValues[2].text = frameRateValue.ToString();

        settingValues[3].text = "ON";

        
    }

    // Update is called once per frame
 
    //private void Update()
    //{
        

       

      
       

       
      

        
       
       
    //}

    public void SettingBGMVolum()
    {
        bgmValue = settingSliderList[0].value / 10;

        settingValues[0].text = (bgmValue * 10).ToString();
    }

    public void SettingSoundVolum()
    {
        soundValue = settingSliderList[1].value / 10;

        settingValues[1].text = (soundValue * 10).ToString();

        soundManager.GetComponent<AudioSource>().volume = soundValue;
    }

    public void SettingFrameRate()
    {
        frameRateValue = (int)settingSliderList[2].value * 60;

        if ((int)settingSliderList[2].value * 60 < 1)
        {
            frameRateValue = 30;
        }

        settingValues[2].text = frameRateValue.ToString();

    }

    /// <summary>
    /// �R���g���[���[���U�����邩���Ȃ�����؂�ւ��郁�\�b�h�ł��B�{�^���Ŏg�p�B
    /// </summary>
    public void VibrationOnOrOff()
    {
        soundManager.OneShotDecisionSound();//����T�E���h

        if (vibrationOn)
        {
            vibrationOn = false;
            settingData.VibrationON = vibrationOn;
            settingValues[3].text = "OFF";
        }
        else
        {
            vibrationOn = true;
            settingData.VibrationON = vibrationOn;
            settingValues[3].text = "ON";
        }
    }

    /// <summary>
    /// settingData�ɂ��ꂼ��̐ݒ�l�����郁�\�b�h�ł��B�V�O�i���Ŏg�p�B
    /// </summary>
    public void SaveSettingValue()
    { 
        settingData.BgmVolume = bgmValue;

        settingData.SoundVolum = soundValue;

        settingData.FrameRate = frameRateValue;

        settingData.CameraRotateSpeed = cameraRotateSpeed;
     

        nowBgmValue = soundManager.MusicManager.GetComponent<AudioSource>().volume;

        if (bgmValue != nowBgmValue)
        { 
        soundManager.MusicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume / 5;
        }
       
        soundManager.GetComponent<AudioSource>().volume = settingData.SoundVolum;

        Application.targetFrameRate = settingData.FrameRate;

        //plain.actions["Angle"].processors
    }

   
}
