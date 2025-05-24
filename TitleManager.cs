using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
/// <summary>
/// �^�C�g���V�[���ł̃C�x���g�𐧌䂷��N���X�ł��B
/// </summary>
public class TitleManager : GameManager
{
    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

    private ButtonSelector buttonSelector;

    private List<PlayableDirector> titlePlayables = new List<PlayableDirector>();

  
    private List<GameObject> selectButton = new List<GameObject>();


    private List<GameObject> selectBackButton = new List<GameObject>();

   
    //private List<ButtonController> buttonConList = new List<ButtonController>();

    [SerializeField]
    private PlayerInput plain;//�C���v�b�g�V�X�e��
    //[SerializeField]
    //private GameObject findpla;

    [SerializeField]
    private GameObject titleTransition;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField] 
    private GameObject eveSys;

    //private SoundManager soundManager;

    //[SerializeField]
    //private GameObject findSoundManager;

    [SerializeField]
    private FlagManagementData gameData;

    [SerializeField]
    private PlayerTransformData playerTransformData;

    private string selectButtonName;

    private string loadSceneName;

    [SerializeField]
    private PlayableDirector saveAdvisMassage;

    private DateSaveUIController dateSaveUIController;

    [SerializeField,Header("dateSaveUIController�̎Q�ƃI�u�W�F�N�g")]
    private GameObject findDSUCon;

    //�f�o�b�N�p
    [SerializeField,Header("�f�o�b�N�p(�D���ȃV�[���Ɉړ�)")]
    private bool debugLoad;
    [SerializeField]
    private List<string> debugLoadSceneNameList = new List<string>();
    [SerializeField]
    private int sceneNumber;
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleTransition.SetActive(true);

        plain = findPla.GetComponent<PlayerInput>();

        buttonSelector = GetComponent<ButtonSelector>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        //gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;
        //soundManager = findSoundManager.GetComponent<SoundManager>();

        dateSaveUIController = findDSUCon.GetComponent<DateSaveUIController>();

        titlePlayables = buttonSelector.ButtonPlayables;

        selectBackButton = buttonSelector.SelsectBackButton;

        selectButton = buttonSelector.SelsectButton;


        if (debugLoad)
        {
            switch (sceneNumber)
            {

                case 0:
                    loadSceneName = debugLoadSceneNameList[0];
                    break;

                case 1:
                    loadSceneName = debugLoadSceneNameList[1];
                    break;

                case 2:
                    loadSceneName = debugLoadSceneNameList[2];
                    break;

                case 3:
                    loadSceneName = debugLoadSceneNameList[3];
                    break;

                case 4:
                    loadSceneName = debugLoadSceneNameList[4];
                    break;

            }
        }
        else
        {
            loadSceneName = "Tutorial";
        }

    }

    //void Update()
    //{
    //    GetSelectButtonName(0);

    //    GetSelectButtonName(1);

    //    GetSelectButtonName(2);

    //    GetSelectButtonName(3);

    //}

    //private void GetSelectButtonName(int num)
    //{
    //    if (buttonConList[num].EventSystem.currentSelectedGameObject.name != null && buttonConList[num].EventSystem.currentSelectedGameObject.name == selectButton[num].name)
    //    {
    //        selectButtonName = buttonConList[num].EventSystem.currentSelectedGameObject.name;
    //    }

    //}

 �@/// <summary>
   ///�V�O�i���Ŏg�p�B�^�C�g�����o�̐���p�B 
   /// </summary>
    public void TitleStart()
    {
       eveSys.SetActive(true);
    }

    /// <summary>
    /// �{�^���Ŏg�p�B�I�����ꂽ�{�^�����ɉ����ă��j���[�̑J�ڂ��s�����\�b�h�ł��B
    /// </summary>
    public void Begging()
    {
        selectButtonName = buttonSelector.SelectButtonName;

        soundManager.OneShotDecisionSound();//����T�E���h

        switch (selectButtonName)
        {
            case "�ŏ�����{�^��":

                titlePlayables[0].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[0]);

                break;

            case "��������{�^��":
                titlePlayables[1].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[1]);

                break;

            case "�I���{�^��":
                titlePlayables[2].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[2]);

                break;

            case "�N���W�b�g�\���{�^��":
                titlePlayables[3].Play();
                EventSystem.current.SetSelectedGameObject(selectBackButton[3]);
                break;
        }
      
    }
    /// <summary>
    /// �V�O�i���Ŏg�p�B�I�����ꂽ�{�^�����ɉ������^�C�����C���̍Đ����~�߂郁�\�b�h�ł��B�B
    /// </summary>
    public void BeggingPause()
    {

      

        switch (selectButtonName)
        {
            case "�ŏ�����{�^��":

                titlePlayables[0].Pause();


                break;

            case "��������{�^��":

                titlePlayables[1].Pause();


                break;

            case "�I���{�^��":
                titlePlayables[2].Pause();

                break;

            case "�N���W�b�g�\���{�^��":
                titlePlayables[3].Pause();

                break;
        }
    }
    /// <summary>
    /// �{�^���Ŏg�p�B���j���[�̃L�����Z�����郁�\�b�h�ł��B
    /// </summary>
    public void BeggingResum()
    {


        soundManager.OneShotCancelSound();//�L�����Z���T�E���h

        switch (selectButtonName)
        {
            case "�ŏ�����{�^��":

                titlePlayables[0].Resume();


                break;

            case "��������{�^��":

                titlePlayables[1].Resume();


                break;

            case "�I���{�^��":
                titlePlayables[2].Resume();

                break;
            case "�N���W�b�g�\���{�^��":
                titlePlayables[3].Resume();

                break;
        }
    }
    /// <summary>
    /// �V�O�i���Ŏg�p�B�L�����Z����A���ꂼ��I�����Ă����{�^���ɃJ�[�\����߂����\�b�h�ł��B
    /// </summary>
    public void ReturnButton()
    {
        switch (selectButtonName)
        {
            case "�ŏ�����{�^��":

                EventSystem.current.SetSelectedGameObject(selectButton[0]);

                break;

            case "��������{�^��":

                EventSystem.current.SetSelectedGameObject(selectButton[1]);

                break;

            case "�I���{�^��":

                EventSystem.current.SetSelectedGameObject(selectButton[2]);

                break;

            case "�N���W�b�g�\���{�^��":

                EventSystem.current.SetSelectedGameObject(selectButton[2]);

                break;
        }

    }

    public void TitleLoadAdvisPause()
    {
      dateSaveUIController.AdvisPause();
    }

    public void TitleLoadAdvisStop() 
    {
    dateSaveUIController.AdvisStop();
    }

    /// <summary>
    /// �{�^���Ŏg�p�B���߂���v���C�����郁�\�b�h�B
    /// </summary>
    public void PlayStrart()
    {
        gameData.SceneName = loadSceneName;
          
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[4].Play();
    }
    /// <summary>
    /// �{�^���Ŏg�p�B��������v���C�����郁�\�b�h�B
    /// </summary>
    public void SuccesionStart()
    {
        soundManager.MusicManager.GetComponent<AudioSource>().Stop();
        soundManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[4].Play();
    }

    
    //public void Loding()
    //{
    //    StartCoroutine(LoadSceneAsync("LoadingScene"));
    //}

    //IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}
    /// <summary>
    /// �Q�[�����I�������郁�\�b�h�B
    /// </summary>
    public void GameQuit()
    {
        soundManager.OneShotDecisionSound();//����T�E���h
        //Invoke("Quit",1f);
        StartCoroutine(Quit());
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
