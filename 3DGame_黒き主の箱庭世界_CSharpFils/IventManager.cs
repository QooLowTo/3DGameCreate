using Fungus;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Fungusを用いたイベントを管理するクラスです。
/// </summary>
public class IventManager : MonoBehaviour
{
   
    [SerializeField]
    private PlayerInput plaIn;//インプットシステム
    [SerializeField]
    private GameObject findPla;

    [SerializeField]
    private List<GameObject> objectList = new List<GameObject>();

    [SerializeField]
    private Flowchart flocha;
    private GameObject findflo;

    [SerializeField]
    private Character character;
    [SerializeField]
    private GameObject targetCharctor;

    [SerializeField]
    private FlagManagementData flagmentData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        findflo = GameObject.FindWithTag("Fungus");

        plaIn = findPla.GetComponent<PlayerInput>();
        flocha = findflo.GetComponent<Flowchart>();
        character = targetCharctor.GetComponent<Character>();

        if (flagmentData.TutorialClear) return;

        flocha.SetBooleanVariable("tutorialClear", flagmentData.TutorialClear);

        StartCoroutine(FirstDialog());
    }

    IEnumerator FirstDialog()
    {
        yield return new WaitForSeconds(3);
        flocha.SendFungusMessage("Center");
    }
    public void GameObjectTrue(int listNum)
    {
        objectList[listNum].SetActive(true);
    }

    public void GameObjectFalse(int listNum)
    {
        objectList[listNum].SetActive(false);
    }

    public void ChageActionUI()
    {
        plaIn.SwitchCurrentActionMap("UI");
    }

    public void ChageActionPlayer()
    {
        plaIn.SwitchCurrentActionMap("Player");
    } 
   
    public void CharactorChangeName()
    {
        character.name = "創造主";
    } 
    public void HomeFlowBooleanTrue()
    {
        flagmentData.TutorialClear = true;
    }

}
