using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// メインのUIの「ステータス」、「設定」などのボタンを制御するクラスです。
/// 選択されたボタンごとに演出するスライドを変えるなどの役割を持っています。
/// </summary>
public class ButtonSelector : UIManager
{
    [SerializeField]
    private string selectButtonName;

    [SerializeField]
    private List<PlayableDirector> buttonPlayables = new List<PlayableDirector>();

    [SerializeField]
    private List<GameObject> selectButton = new List<GameObject>();

    [SerializeField]
    private List<GameObject> selectBackButton = new List<GameObject>();

    [SerializeField]
    private List<ButtonController> buttonConList = new List<ButtonController>();


    [SerializeField]
    protected FlagManagementData flagManagementData;


    public List<GameObject> SelectButton { get => selectButton; set => selectButton = value; }
    public string SelectButtonName { get => selectButtonName; set => selectButtonName = value; }
    public List<PlayableDirector> ButtonPlayables { get => buttonPlayables; set => buttonPlayables = value; }
    public List<GameObject> SelectButton { get => selectButton; set => selectButton = value; }
    public List<GameObject> SelectBackButton { get => selectBackButton; set => selectBackButton = value; }

    void Start()
    {
        
        StartUISetting();
    }

    /// <summary>
    /// 選択しているボタンを取得するメソッドです。ButtonControllerで使用。
    /// </summary>
    public void GetSelectButtonName()
    {
        for (int i = 0; i < buttonConList.Count; i++)
        {
            //選択中のオブジェクトの名前がnullでないかつ
            //指定のリストの名前と一致いていたら。
            if (buttonConList[i].EventSystem.currentSelectedGameObject.name != null &&
                buttonConList[i].EventSystem.currentSelectedGameObject.name == selectButton[i].name)
            {

                selectButtonName = buttonConList[i].EventSystem.currentSelectedGameObject.name;//名前を格納する。

            }
        }

    }

}
