using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

/// <summary>
/// メインのUIの「ステータス」、「設定」などのボタンを制御するクラスです。
/// 選択されたボタンごとに演出するスライドを変えるなどの役割を持っています。
/// </summary>
public class ButtonOpen : MonoBehaviour
{
    [SerializeField]
    private List<PlayableDirector> buttonPlayables = new List<PlayableDirector>(); //説明書きましょう

    [SerializeField]
    private List<GameObject> selsectButton = new List<GameObject>();//説明書きましょう

    [SerializeField]
    private List<GameObject> selsectBackButton = new List<GameObject>();//説明書きましょう

    [SerializeField]
    private List<ButtonController> buttonConList = new List<ButtonController>();//説明書きましょう

    [SerializeField]
    private GameObject mainEveSys;//説明書きましょう

    [SerializeField]
    private GameObject subEveSys;//説明書きましょう

    [SerializeField]
    private GameManager GM; //ゲームマネージャー
    
    [SerializeField]
    private GameObject FindGM;//ゲームマネージャーを探す用

    ButtonController buttonController; //説明書きましょう
    private GameObject findButtonCntr; //説明書きましょう


    private bool openNow = false; //説明書きましょう


    private bool cancelOK = false; //UIコントローラーで使う //説明書きましょう

    [SerializeField]
    private string selectButtonName; //説明書きましょう

    public bool OpenNow { get => openNow; set => openNow = value; }
    public bool CancelOK { get => cancelOK; set => cancelOK = value; }
    public string SelectButtonName { get => selectButtonName; set => selectButtonName = value; }

    void Start()
    {
        //findButtonCntr = GameObject.FindWithTag("SelectButton");

        //buttonController = findButtonCntr.GetComponent<ButtonController>();

        GM = FindGM.GetComponent<GameManager>();
       
    }

    void Update()
    {

        if (buttonConList[0].EventSystem.currentSelectedGameObject.name != null && buttonConList[0].EventSystem.currentSelectedGameObject.name == "Status")
        {
            selectButtonName = buttonConList[0].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[1].EventSystem.currentSelectedGameObject.name != null && buttonConList[1].EventSystem.currentSelectedGameObject.name == "仮")
        {
            selectButtonName = buttonConList[1].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[5].EventSystem.currentSelectedGameObject.name != null && buttonConList[5].EventSystem.currentSelectedGameObject.name == "Setting")
        {
            selectButtonName = buttonConList[5].EventSystem.currentSelectedGameObject.name;
        }

    }

    public void SlideOpen()
    {
       
        GM.Decision_Sound();

        switch (selectButtonName)
        {
            case "Status":

                buttonPlayables[0].Play();
                //buttonConList[0].EventSystem.enabled = false;
             
                mainEveSys.SetActive(false);
               

            break;

            case "Setting":
                buttonPlayables[5].Play();
             
                mainEveSys.SetActive(false);
            break;
        }
    }

    public void SlidePause()//シグナルで制御
    {  
        openNow = true;

        cancelOK = true;

        switch (selectButtonName)
        {
            case "Status":

            buttonPlayables[0].Pause();
     
            subEveSys.SetActive(true);

            //subEveSys.GetComponent<EventSystem>().firstSelectedGameObject = selsectBackButton[0];

                break;

            case "Setting":

                buttonPlayables[5].Pause();

                subEveSys.SetActive(true);

                //subEveSys.GetComponent<EventSystem>().firstSelectedGameObject = selsectBackButton[1];

            break;
        }

    }

    public void SlideResume()//ボタン
    {
        GM.Cancel_Sound();

        switch (selectButtonName)
        {
            case "Status":

                //buttonConList[0].EventSystem.enabled = false;
              
                buttonPlayables[0].Resume();

                break;

            case "Setting":

                //buttonConList[0].EventSystem.enabled = false;

                buttonPlayables[5].Resume();

                break;
        }
    }

    public void SlideClose()//シグナルで制御
    { 
        
        switch (selectButtonName)
        {
            case "Status":

                subEveSys.SetActive(false);
                mainEveSys.SetActive(true);
                mainEveSys.GetComponent<EventSystem>().firstSelectedGameObject = selsectButton[0];

                break;

            case "Setting":

                subEveSys.SetActive(false);
                mainEveSys.SetActive(true);
                mainEveSys.GetComponent<EventSystem>().firstSelectedGameObject = selsectButton[5];

                break;
        }

        openNow = false;
        cancelOK = false;
    }

    
}
