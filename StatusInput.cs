using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ���C��UI�̃X�e�[�^�X���ڂ̊e��X�e�[�^�X�Ɍ��݂̃X�e�[�^�X�𔽉f������N���X�ł��B
/// </summary>
public class StatusInput : UIManager
{
    [SerializeField]
    private List<TextMeshProUGUI> statusList = new List<TextMeshProUGUI>();

    //private UIManager uiManager;

    //private GameObject findUiManager;

    //[SerializeField]
    //private StatusDate statusDate;
    //[SerializeField]
    //private ExpManager expManager;
    // Start is called before the first frame update
    void Start()
    {
       
        StartUISetting();
    }

    // Update is called once per frame
    void Update()
    {
        
        statusList[0].text = "<size=90>L</size>v.<size=100>" + plaSCon.PlayerLevel.ToString();

        statusList[1].text = "<size=75>H</size>P:<size=80>" + plaSCon.PlayerHP.ToString();

        statusList[2].text = "<size=75>�U</size>����:<size=80>" + plaSCon.PlayerAttackPower.ToString();

        statusList[3].text = "<size=75>�h</size>���:<size=80>" + plaSCon.PlayerDefance.ToString();

        statusList[4].text = "<size=75>E</size>xp:<size=60>" + plaSCon.PlayerExp.ToString();

        statusList[5].text = "���̃��x���܂�<size=60>"+ (expManager.ExpTablesList[statusDate.D_ExpListElement] - plaSCon.PlayerExp).ToString() + "</size>Exp";
    }
}
