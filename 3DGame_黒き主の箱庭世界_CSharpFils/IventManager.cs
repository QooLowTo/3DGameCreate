using Fungus;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Fungusを用いたイベントを管理するクラスです。
/// </summary>
public class EventManager : MonoBehaviour
{
   
    [SerializeField]
    private PlayerInput playerInput; //インプットシステム
    [SerializeField]
    private GameObject findPlayerObj;

    [SerializeField]
    private List<GameObject> objectList = new List<GameObject>();

    [SerializeField]
    private Flowchart flowchart;
    private GameObject findFlowchartObj;

    [SerializeField]
    private Character character;
    [SerializeField]
    private GameObject targetCharacter;

    [SerializeField]
    private FlagManagementData flagManagementData;

    void Start()
    {
        findFlowchartObj = GameObject.FindWithTag("Fungus");

        playerInput = findPlayerObj.GetComponent<PlayerInput>();
        flowchart = findFlowchartObj.GetComponent<Flowchart>();
        character = targetCharctor.GetComponent<Character>();

        if (flagManagementData.TutorialClear) return;

        flowchart.SetBooleanVariable("tutorialClear", flagManagementData.TutorialClear);

        StartCoroutine(FirstDialog());
    }

    /// <summary>
    /// 説明書いて
    /// </summary>
    /// <returns></returns>

    IEnumerator FirstDialog()
    {
        //マジックナンバー
        yield return new WaitForSeconds(3);
        flowchart.SendFungusMessage("Center");
    }

    /// <summary>
    /// 説明書いて
    /// </summary>
    /// <param name="listNum"></param>
    public void GameObjectTrue(int listNum)
    {
        objectList[listNum].SetActive(true);
    }

    public void GameObjectFalse(int listNum)
    {
        objectList[listNum].SetActive(false);
    }

    public void ChangeActionUI()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void ChangeActionPlayer()
    {
        playerInput.SwitchCurrentActionMap("Player");
    } 
   
    public void CharacterChangeName()
    {
        character.name = "創造主";
    } 
    public void HomeFlowBooleanTrue()
    {
        flagManagementData.TutorialClear = true;
    }

}
