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
public class TitleEvent : MonoBehaviour
{
    [SerializeField]
    private List<PlayableDirector> titlePlayables = new List<PlayableDirector>();

    [SerializeField]
    private PlayerInput playerInput;//�C���v�b�g�V�X�e��

    [SerializeField]
    private EventSystem eve;

    [SerializeField]
    private GameObject loadSceneObject;

    [SerializeField]
    private GameObject biginCancelButton; //���p�H�������Ă�������

    [SerializeField]
    private GameObject biggingButton;�@//���p�H�������Ă�������

    //[SerializeField] 
    //private GameObject titleButtons;

    [SerializeField]
    private GameObject mainEveSys;

    [SerializeField]
    private GameObject subEveSys;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject findGame;

    [SerializeField]
    private FlagManagementData
     gameData;

    [SerializeField]
    private string loadSceneName;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        gameManager = findGame.GetComponent<GameManager>();

        eve = EventSystem.current;
    }

    /// <summary>
    /// �C�x���g���J�n���܂��B
    /// </summary>
    public void Begging() //Begging���ƃX�y���~�X���Ă���̂ŁABegging��Begin�ɕύX���Ă�������
    {
        gameManager.Decision_Sound();
        mainEveSys.SetActive(false);
        titlePlayables[0].Play();
    }

    /// <summary>
    /// �C�x���g���ꎞ��~���܂��B
    /// </summary>
    public void BeggingPause() //BeginPause�ɂ������������B���ꂩ�ABeginningPause
    {
        subEveSys.SetActive(true);
        //titleButtons.SetActive(false);
        titlePlayables[0].Pause(); 
    }

    /// <summary>
    /// �C�x���g���ĊJ���܂��B
    /// </summary>
    public void BeggingResum() //BeginResume��������BeginningResume
    {
        gameManager.Cancel_Sound();
        //titleButtons.SetActive(true);
    
        titlePlayables[0].Resume();
    }

    /// <summary>
    /// �C�x���g���A�N�e�B�u�ɂ��܂��B
    /// </summary>
    public void EventActive()
    { 
        eve.firstSelectedGameObject = biggingButton;
        subEveSys.SetActive(false);
        mainEveSys.SetActive(true);
    }

    /// <summary>
    /// �Q�[�����J�n���܂��B
    /// </summary>
    public void Begin()
    {
        gameData.SceneName = loadSceneName;
        gameManager.MusicManager.GetComponent<AudioSource>().Stop();
        gameManager.LoadingStart_Sound();
        loadSceneObject.SetActive(true);
        titlePlayables[1].Play();
    }


        /// <summary>
        /// �V�[�������[�h���܂��B
        /// </summary>
    public void Loading()
    {
        StartCoroutine(LoadSceneAsync("LoadingScene"));
    }

    /// <summary>
    /// �V�[����񓯊��Ń��[�h���܂��B
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
