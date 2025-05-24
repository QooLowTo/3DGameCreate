using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        // �O�̃��[�h�V�[�����A�����[�h
        SceneManager.UnloadSceneAsync("FirstLoading");

        // ���̃V�[����񓯊��œǂݍ���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
