using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム起動時にデータファイルからセーブデータを読み取るクラスです。
/// </summary>
public class firstGameLoading : MonoBehaviour
{
    [SerializeField]
    private SaveLoadSystem saveLoadSystem;
    [SerializeField]
    private GameObject findSaveandLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLoadSystem = findSaveandLoad.GetComponent<SaveLoadSystem>();

        saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData1.sqlite";
        saveLoadSystem.FirstLoad();

        saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData2.sqlite";
        saveLoadSystem.FirstLoad();

        saveLoadSystem.FileName = "URI=file:HakoniwaSekaiSaveData3.sqlite";
        saveLoadSystem.FirstLoad();

        StartCoroutine(LoadNextSceneAsync());
    }

    // Update is called once per frame
    IEnumerator LoadNextSceneAsync()
    {
        // 前のロードシーンをアンロード
        SceneManager.UnloadSceneAsync("FirstLoading");

        // 次のシーンを非同期で読み込み
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
