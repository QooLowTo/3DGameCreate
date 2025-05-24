using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoading : MonoBehaviour
{
    [SerializeField]
    private StatusDate status;
    [SerializeField]
    private ExpManager expManager;
    [SerializeField] 
    private FlagManagementData flagmentDate;
    [SerializeField]
    private PlayerTransformData playerTransform;
    [SerializeField]
    private SettingData settingData;
    [SerializeField]
    private MemorizeLevelAndSceneName memorizeLevelAndSceneName;

    [SerializeField] 
    private GameObject titleBackUI;
    // Start is called before the first frame update
    void Start()
    {
        if (flagmentDate.SceneName == "Title")
        {
            titleBackUI.SetActive(true);
        }

        StartCoroutine(LoadNextSceneAsync());
    }

    // Update is called once per frame
    IEnumerator LoadNextSceneAsync()
    {
        // �O�̃��[�h�V�[�����A�����[�h
        //SceneManager.UnloadSceneAsync("LoadingScene");

        // ���̃V�[����񓯊��œǂݍ���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(flagmentDate.SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
