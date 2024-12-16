using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲームのロード画面を制御するクラスです。
/// </summary>
public class GameLoading : MonoBehaviour
{
    [SerializeField] private FlagManagementData flagManagementData;

    [SerializeField] private Slider slider; //アンダーバーにする命名規則ですか？ならば他の変数もそうしましょう　じゃないと統一性がない
    [SerializeField] private GameObject loadingUI;

    void Start()
    {
        loadingUI.SetActive(true);
        StartCoroutine(LoadNextSceneAsync());
    }

/// <summary>
/// 次のシーンを非同期で読み込むメソッドです。
/// </summary>
/// <returns></returns>
    IEnumerator LoadNextSceneAsync()
    {
        // 前のロードシーンをアンロード
        SceneManager.UnloadSceneAsync("LoadingScene");

        // 次のシーンを非同期で読み込み
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(flagManagementData.SceneName);

        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress;
            yield return null;
        }
    }
}
