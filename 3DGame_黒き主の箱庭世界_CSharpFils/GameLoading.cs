using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 次のシーンを非同期でローディングするクラスです。
/// </summary>
public class GameLoading : MonoBehaviour
{
    [SerializeField]
    private StatusDate status;
    [SerializeField]
    private ExpManager expManager;
    [SerializeField] 
    private FlagManagementData flagManagementData;
    [SerializeField]
    private PlayerTransformData playerTransform;
    [SerializeField]
    private SettingData settingData;
    [SerializeField]
    private MemorizeLevelAndSceneName memorizeLevelAndSceneName;

    [SerializeField] 
    private GameObject titleBackUI;
  
    void Start()
    {
        if (flagManagementData.SceneName == "Title")
        {
            titleBackUI.SetActive(true);
        }

        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
     

        // 次のシーンを非同期で読み込み
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(flagManagementData.SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
