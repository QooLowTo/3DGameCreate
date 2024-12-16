using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Q�[���̃��[�h��ʂ𐧌䂷��N���X�ł��B
/// </summary>
public class GameLoading : MonoBehaviour
{
    [SerializeField] private FlagManagementData flagManagementData;

    [SerializeField] private Slider slider; //�A���_�[�o�[�ɂ��閽���K���ł����H�Ȃ�Α��̕ϐ����������܂��傤�@����Ȃ��Ɠ��ꐫ���Ȃ�
    [SerializeField] private GameObject loadingUI;

    void Start()
    {
        loadingUI.SetActive(true);
        StartCoroutine(LoadNextSceneAsync());
    }

/// <summary>
/// ���̃V�[����񓯊��œǂݍ��ރ��\�b�h�ł��B
/// </summary>
/// <returns></returns>
    IEnumerator LoadNextSceneAsync()
    {
        // �O�̃��[�h�V�[�����A�����[�h
        SceneManager.UnloadSceneAsync("LoadingScene");

        // ���̃V�[����񓯊��œǂݍ���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(flagManagementData.SceneName);

        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress;
            yield return null;
        }
    }
}
