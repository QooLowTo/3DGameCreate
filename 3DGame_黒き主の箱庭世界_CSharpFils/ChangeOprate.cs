using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeOprate : UIManager
{
    [SerializeField]
    private List<GameObject> operateText = new List<GameObject>();

    [SerializeField]
    private List<TextMeshProUGUI> changeText = new List<TextMeshProUGUI>();

    //private UIManager uiManager;

    //private GameObject findUiManager;

    bool oprateChange = false;

    Color offColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUISetting();

        offColor = changeText[1].color;
    }

    // Update is called once per frame
    
    public void ChangeOp()
    {
        soundManager.OneShotDecisionSound();//����T�E���h

        if (!oprateChange)
        {
            operateText[0].SetActive(false);

            operateText[1].SetActive(true);

            changeText[0].color = offColor;

            changeText[0].text = "�L�[�{�[�h";

            changeText[1].color = Color.white;

            changeText[1].text = "���R���g���[���[��";

            oprateChange = true;
        }
        else
        {
            operateText[1].SetActive(false);

            operateText[0].SetActive(true);

            changeText[0].color = Color.white;

            changeText[0].text = "���L�[�{�[�h��";

            changeText[1].color = offColor;

            changeText[1].text = "�R���g���[���[";

            oprateChange =false;
        }
    }
}
