using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
/// <summary>
/// データをセーブするUIを制御するクラスです。
/// </summary>
public class DateSaveUIController : UIManager
{
    [SerializeField]
    private List<ButtonController> buttonConList = new List<ButtonController>();

    [SerializeField]
    private List<GameObject> saveButtonList = new List<GameObject>();

    [SerializeField]
    private List<TextMeshProUGUI> inputDataTextList = new List<TextMeshProUGUI>();

    [SerializeField]
    private string selectDataName;

    private bool advisOpen = false;

    //private UIManager uiManager;

    //private GameObject findUiManager;

    //private GameObject findGM;

    //private GameObject findSaveLoad;
   
    //private SaveLoadSystem saveLoadSystem;

    //private GameManager gameManager;

    [SerializeField]
    private PlayableDirector saveAdvisMassage;
    [SerializeField]
    private TextMeshProUGUI AdvisDataSlotNumText;
    [SerializeField]
    private GameObject selectButton;

    [SerializeField]
    private FlagManagementData flagmentData;

    //[SerializeField]
    //private StatusDate statusDate;

    [SerializeField]
    private MemorizeLevelAndSceneName memorizeLevelAndSceneName;

    public bool SaveAdvisOpen { get => advisOpen; set => advisOpen = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartUISetting();
        //findUiManager = GameObject.FindWithTag("UIManager");

        //findGM = GameObject.FindWithTag("GameManager");

        //findSaveLoad = GameObject.FindWithTag("SaveAndLoad");

        //uiManager = findUiManager.GetComponent<UIManager>();

        //gameManager = findGM.GetComponent<GameManager>();
        saveLoadSystem = findSaveLoad.GetComponent<SaveLoadSystem>();

      StartInputSaveData();

    }


    // Update is called once per frame
    void Update()
    {
        if (saveButtonList[0] == null) return;

        if (buttonConList[0].EventSystem.currentSelectedGameObject.name != null && buttonConList[0].EventSystem.currentSelectedGameObject.name == "データ1")
        {
            selectDataName = buttonConList[0].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[1].EventSystem.currentSelectedGameObject.name != null && buttonConList[1].EventSystem.currentSelectedGameObject.name == "データ2")
        {
            selectDataName = buttonConList[1].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[2].EventSystem.currentSelectedGameObject.name != null && buttonConList[2].EventSystem.currentSelectedGameObject.name == "データ3")
        {
            selectDataName = buttonConList[2].EventSystem.currentSelectedGameObject.name;
        }
    }

    private void StartInputSaveData()//スタート時に呼び出す。
    {
        if (flagmentData.Data1Exist)
        {
            inputDataTextList[0].text = "Lv." +memorizeLevelAndSceneName.SavedPlayerLevelList[0];
            inputDataTextList[1].text = "□セーブ地点:";
            inputDataTextList[2].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[0]).ToString();
        }

        if (flagmentData.Data2Exist)
        {
            inputDataTextList[3].text = "Lv." + memorizeLevelAndSceneName.SavedPlayerLevelList[1];
            inputDataTextList[4].text = "□セーブ地点:";
            inputDataTextList[5].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[1]).ToString();
        }

        if (flagmentData.Data3Exist)
        {
            inputDataTextList[6].text = "Lv." + memorizeLevelAndSceneName.SavedPlayerLevelList[2];
            inputDataTextList[7].text = "□セーブ地点:";
            inputDataTextList[8].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[2]).ToString();
        }
    }


    public void SelectDataName()
    {
       
        switch (selectDataName)
        {
         
            case "データ1":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData1.sqlite";
            break;

            case "データ2":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData2.sqlite";
            break;

            case "データ3":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData3.sqlite";
            break;
        }
    }


    public void SendSaveAdvice()//忠告メッセージ表示
    {
        soundManager.OneShotDecisionSound();//決定サウンド

        advisOpen = true;

        switch (selectDataName)
        {

            case "データ1":

                //if (!flagmentData.SaveData1Exist) return;
                AdvisDataSlotNumText.text = "<u>セーブデータスロットI</u>に保存しますか？";
                
                break;

            case "データ2":

                //if (!flagmentData.SaveData2Exist) return;
                AdvisDataSlotNumText.text = "<u>セーブデータスロットII</u>に保存しますか？";

                break;

            case "データ3":

                //if (!flagmentData.SaveData3Exist) return;
                AdvisDataSlotNumText.text = "<u>セーブデータスロットIII</u>に保存しますか？";

                break;


        }  
        
        saveAdvisMassage.Play();//忠告メッセージを表示

    }

    public void SendLoadAdvice()//忠告メッセージ表示
    {

        switch (selectDataName)
        {

            case "データ1":

                if (!flagmentData.Data1Exist)
                {
                    soundManager.OneShotCancelSound();//キャンセルサウンド
                }
                else 
                {
                 soundManager.OneShotDecisionSound();//決定サウンド
                    advisOpen = true;
                AdvisDataSlotNumText.text = "<u>セーブデータスロットI</u>を読み込みますか？";
                saveAdvisMassage.Play();//忠告メッセージを表示
                }
               
                break;

            case "データ2":

                if (!flagmentData.Data2Exist)
                {
                    soundManager.OneShotCancelSound();//キャンセルサウンド
                }
                else
                {
                    soundManager.OneShotDecisionSound();//決定サウンド
                    advisOpen = true;
                    AdvisDataSlotNumText.text = "<u>セーブデータスロットII</u>を読み込みますか？";
                    saveAdvisMassage.Play();//忠告メッセージを表示
                }

                break;

            case "データ3":

                if (!flagmentData.Data3Exist)
                {
                    soundManager.OneShotCancelSound();//キャンセルサウンド
                }
                else
                {
                    soundManager.OneShotDecisionSound();//決定サウンド
                    advisOpen = true;
                    AdvisDataSlotNumText.text = "<u>セーブデータスロットIII</u>を読み込みますか？";
                    saveAdvisMassage.Play();//忠告メッセージを表示
                }

                break;


        }

     

    }

    public void GetSaveData()
    {
        switch (selectDataName)
        {

            case "データ1":

                if (flagmentData.Data1Exist) return;
                 
                flagmentData.Data1Exist = true;

                break;

            case "データ2":

                if (flagmentData.Data2Exist) return;

                flagmentData.Data2Exist = true;

                break;

            case "データ3":

                if (flagmentData.Data3Exist) return;

                flagmentData.Data3Exist = true;

                break;


        }
    }

   

    public void InputDataLvAndSceneName()//セーブ時に値を更新、また、値を別スクリプタブに保存。(セーブ選択時「はい」で呼ぶ)
    {

        switch (selectDataName)
        {
            case "データ1":

                InputData(0,1,2,0);

                break;

            case "データ2":

                InputData(3,4,5,1);

                break;

            case "データ3":

                InputData(6,7,8,2);

                break;

        }
 
    }
    /// <summary>
    /// 記録したデータの情報を表示します。引数には、「テキストの頭のナンバー」、
    /// 「メモライズオブジェクト（スクリプタブ）のリストの番号」を入れてください。
    /// </summary>
    /// <param name="textNum"></param>
    /// <param name="memoNum"></param>
    private void InputData(int textNum1,int textNum2, int textNum3, int memoNum)
    {
        inputDataTextList[textNum1].text = "Lv." + statusDate.D_PlayerLevel.ToString();
        inputDataTextList[textNum2].text = "□セーブ地点:";
        inputDataTextList[textNum3].text = "<u>" + NomarizeSceneName(flagmentData.SceneName).ToString();

        memorizeLevelAndSceneName.SavedPlayerLevelList[memoNum] = statusDate.D_PlayerLevel.ToString();
        memorizeLevelAndSceneName.SavedSceneNameList[memoNum] = NomarizeSceneName(flagmentData.SceneName).ToString();
    }

    private string NomarizeSceneName(string MapName)
    {
       
        switch (MapName)
        {
            case "HomeMap":

                MapName = "世界の中心";

            break;

            case "Map1":

                MapName = "緑の箱庭";

                break;

            case "BossArea1":

                MapName = "緑の支配者の間";

                break;

            case "Map2":

                MapName = "青の箱庭";

                break;

            case "Map3":

                MapName = "世界の中心";

                break;
        }


        return MapName;
    }

    public void AdvisPause()
    { 

    saveAdvisMassage.GetComponent<PlayableDirector>().Pause();

    EventSystem.current.SetSelectedGameObject(selectButton);

    }

    public void AdvisResume()
    {
        soundManager.OneShotCancelSound();//キャンセルサウンド

        saveAdvisMassage.GetComponent<PlayableDirector>().Resume();
    }

    public void AdvisStop()
    {
        advisOpen = false;

        switch (selectDataName)
        {

            case "データ1":

                EventSystem.current.SetSelectedGameObject(saveButtonList[0]);

                break;

            case "データ2":

                EventSystem.current.SetSelectedGameObject(saveButtonList[1]);

                break;

            case "データ3":

                EventSystem.current.SetSelectedGameObject(saveButtonList[2]);

                break;


        }
    }
}
