using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 操作説明にて、キーボード操作かコントローラー操作かを切り替えて表示させれるようにするクラスです。
/// </summary>
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
        soundManager.OneShotDecisionSound();//決定サウンド

        if (!oprateChange)
        {
            operateText[0].SetActive(false);

            operateText[1].SetActive(true);

            changeText[0].color = offColor;

            changeText[0].text = "キーボード";

            changeText[1].color = Color.white;

            changeText[1].text = "□コントローラー□";

            oprateChange = true;
        }
        else
        {
            operateText[1].SetActive(false);

            operateText[0].SetActive(true);

            changeText[0].color = Color.white;

            changeText[0].text = "□キーボード□";

            changeText[1].color = offColor;

            changeText[1].text = "コントローラー";

            oprateChange =false;
        }
    }
}
