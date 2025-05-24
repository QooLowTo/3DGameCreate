using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

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

        if (buttonConList[0].EventSystem.currentSelectedGameObject.name != null && buttonConList[0].EventSystem.currentSelectedGameObject.name == "�f�[�^1")
        {
            selectDataName = buttonConList[0].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[1].EventSystem.currentSelectedGameObject.name != null && buttonConList[1].EventSystem.currentSelectedGameObject.name == "�f�[�^2")
        {
            selectDataName = buttonConList[1].EventSystem.currentSelectedGameObject.name;
        }

        if (buttonConList[2].EventSystem.currentSelectedGameObject.name != null && buttonConList[2].EventSystem.currentSelectedGameObject.name == "�f�[�^3")
        {
            selectDataName = buttonConList[2].EventSystem.currentSelectedGameObject.name;
        }
    }

    private void StartInputSaveData()//�X�^�[�g���ɌĂяo���B
    {
        if (flagmentData.Data1Exist)
        {
            inputDataTextList[0].text = "Lv." +memorizeLevelAndSceneName.SavedPlayerLevelList[0];
            inputDataTextList[1].text = "���Z�[�u�n�_:";
            inputDataTextList[2].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[0]).ToString();
        }

        if (flagmentData.Data2Exist)
        {
            inputDataTextList[3].text = "Lv." + memorizeLevelAndSceneName.SavedPlayerLevelList[1];
            inputDataTextList[4].text = "���Z�[�u�n�_:";
            inputDataTextList[5].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[1]).ToString();
        }

        if (flagmentData.Data3Exist)
        {
            inputDataTextList[6].text = "Lv." + memorizeLevelAndSceneName.SavedPlayerLevelList[2];
            inputDataTextList[7].text = "���Z�[�u�n�_:";
            inputDataTextList[8].text = "<u>" + NomarizeSceneName(memorizeLevelAndSceneName.SavedSceneNameList[2]).ToString();
        }
    }


    public void SelectDataName()
    {
       
        switch (selectDataName)
        {
         
            case "�f�[�^1":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData1.sqlite";
            break;

            case "�f�[�^2":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData2.sqlite";
            break;

            case "�f�[�^3":
                saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData3.sqlite";
            break;
        }
    }


    public void SendSaveAdvice()//�������b�Z�[�W�\��
    {
        soundManager.OneShotDecisionSound();//����T�E���h

        advisOpen = true;

        switch (selectDataName)
        {

            case "�f�[�^1":

                //if (!flagmentData.SaveData1Exist) return;
                AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gI</u>�ɕۑ����܂����H";
                
                break;

            case "�f�[�^2":

                //if (!flagmentData.SaveData2Exist) return;
                AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gII</u>�ɕۑ����܂����H";

                break;

            case "�f�[�^3":

                //if (!flagmentData.SaveData3Exist) return;
                AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gIII</u>�ɕۑ����܂����H";

                break;


        }  
        
        saveAdvisMassage.Play();//�������b�Z�[�W��\��

    }

    public void SendLoadAdvice()//�������b�Z�[�W�\��
    {

        switch (selectDataName)
        {

            case "�f�[�^1":

                if (!flagmentData.Data1Exist)
                {
                    soundManager.OneShotCancelSound();//�L�����Z���T�E���h
                }
                else 
                {
                 soundManager.OneShotDecisionSound();//����T�E���h
                    advisOpen = true;
                AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gI</u>��ǂݍ��݂܂����H";
                saveAdvisMassage.Play();//�������b�Z�[�W��\��
                }
               
                break;

            case "�f�[�^2":

                if (!flagmentData.Data2Exist)
                {
                    soundManager.OneShotCancelSound();//�L�����Z���T�E���h
                }
                else
                {
                    soundManager.OneShotDecisionSound();//����T�E���h
                    advisOpen = true;
                    AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gII</u>��ǂݍ��݂܂����H";
                    saveAdvisMassage.Play();//�������b�Z�[�W��\��
                }

                break;

            case "�f�[�^3":

                if (!flagmentData.Data3Exist)
                {
                    soundManager.OneShotCancelSound();//�L�����Z���T�E���h
                }
                else
                {
                    soundManager.OneShotDecisionSound();//����T�E���h
                    advisOpen = true;
                    AdvisDataSlotNumText.text = "<u>�Z�[�u�f�[�^�X���b�gIII</u>��ǂݍ��݂܂����H";
                    saveAdvisMassage.Play();//�������b�Z�[�W��\��
                }

                break;


        }

     

    }

    public void GetSaveData()
    {
        switch (selectDataName)
        {

            case "�f�[�^1":

                if (flagmentData.Data1Exist) return;
                 
                flagmentData.Data1Exist = true;

                break;

            case "�f�[�^2":

                if (flagmentData.Data2Exist) return;

                flagmentData.Data2Exist = true;

                break;

            case "�f�[�^3":

                if (flagmentData.Data3Exist) return;

                flagmentData.Data3Exist = true;

                break;


        }
    }

   

    public void InputDataLvAndSceneName()//�Z�[�u���ɒl���X�V�A�܂��A�l��ʃX�N���v�^�u�ɕۑ��B(�Z�[�u�I�����u�͂��v�ŌĂ�)
    {

        switch (selectDataName)
        {
            case "�f�[�^1":

                InputData(0,1,2,0);

                break;

            case "�f�[�^2":

                InputData(3,4,5,1);

                break;

            case "�f�[�^3":

                InputData(6,7,8,2);

                break;

        }
 
    }
    /// <summary>
    /// �L�^�����f�[�^�̏���\�����܂��B�����ɂ́A�u�e�L�X�g�̓��̃i���o�[�v�A
    /// �u�������C�Y�I�u�W�F�N�g�i�X�N���v�^�u�j�̃��X�g�̔ԍ��v�����Ă��������B
    /// </summary>
    /// <param name="textNum"></param>
    /// <param name="memoNum"></param>
    private void InputData(int textNum1,int textNum2, int textNum3, int memoNum)
    {
        inputDataTextList[textNum1].text = "Lv." + statusDate.D_PlayerLevel.ToString();
        inputDataTextList[textNum2].text = "���Z�[�u�n�_:";
        inputDataTextList[textNum3].text = "<u>" + NomarizeSceneName(flagmentData.SceneName).ToString();

        memorizeLevelAndSceneName.SavedPlayerLevelList[memoNum] = statusDate.D_PlayerLevel.ToString();
        memorizeLevelAndSceneName.SavedSceneNameList[memoNum] = NomarizeSceneName(flagmentData.SceneName).ToString();
    }

    private string NomarizeSceneName(string MapName)
    {
       
        switch (MapName)
        {
            case "HomeMap":

                MapName = "���E�̒��S";

            break;

            case "Map1":

                MapName = "�΂̔���";

                break;

            case "BossArea1":

                MapName = "�΂̎x�z�҂̊�";

                break;

            case "Map2":

                MapName = "�̔���";

                break;

            case "Map3":

                MapName = "���E�̒��S";

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
        soundManager.OneShotCancelSound();//�L�����Z���T�E���h

        saveAdvisMassage.GetComponent<PlayableDirector>().Resume();
    }

    public void AdvisStop()
    {
        advisOpen = false;

        switch (selectDataName)
        {

            case "�f�[�^1":

                EventSystem.current.SetSelectedGameObject(saveButtonList[0]);

                break;

            case "�f�[�^2":

                EventSystem.current.SetSelectedGameObject(saveButtonList[1]);

                break;

            case "�f�[�^3":

                EventSystem.current.SetSelectedGameObject(saveButtonList[2]);

                break;


        }
    }
}
